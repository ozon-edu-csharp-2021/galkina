using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Configuration;
using OzonEdu.MerchandiseService.Infrastructure.Handlers.Aggregate.RequestsToStockApi;
using OzonEdu.StockApi.Grpc;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Aggregate
{
    public class GiveUnreleasedMerchPacksCommandHandler : IRequestHandler<GiveUnreleasedMerchPacksCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly StockApiGrpc.StockApiGrpcClient _stockServiceClient;
        private static IConfiguration _configuration;

        public GiveUnreleasedMerchPacksCommandHandler(IMerchPackRepository merchPackRepository, IUnitOfWork unitOfWork,
            StockApiGrpc.StockApiGrpcClient stockServiceClient, IConfiguration configuration)
        {
            _merchPackRepository = merchPackRepository;
            _unitOfWork = unitOfWork;
            _stockServiceClient = stockServiceClient;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(GiveUnreleasedMerchPacksCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);

            List<MerchPack> merchPacks =
                await _merchPackRepository.GetMerchPacksAwaitedDeliveryAsync(cancellationToken);

            if (merchPacks is not null)
            {
                foreach (var merchPack in merchPacks)
                {
                    if (IsYetEmployee(merchPack.Employee.Id))
                    {
                        bool isSkusAvailable = await RequestToStockApi.DoesSkusAvailable(_stockServiceClient, merchPack.Items.Skues, cancellationToken);

                        Console.WriteLine("This is isSkusAvailable" + isSkusAvailable);
                        if (isSkusAvailable)
                        {
                            merchPack.StockConfirm();
                            if (await RequestToStockApi.ReserveSkus(_stockServiceClient, merchPack.Items.Skues, cancellationToken))
                                merchPack.StockReserve();
                            else
                                merchPack.StockAwaitDelivery();
                        }
                    }
                    else
                    {
                        merchPack.Cancel();
                    }

                    _merchPackRepository.UpdateMerchPackAsync(merchPack, cancellationToken);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        
        private static bool IsYetEmployee(long employeeId)
        {
            var connectionAddress = _configuration.GetSection(nameof(EmployeesServiceConfiguration))
                .Get<EmployeesServiceConfiguration>().ServerAddress;
            if(string.IsNullOrWhiteSpace(connectionAddress))
                connectionAddress = _configuration
                    .Get<EmployeesServiceConfiguration>()
                    .ServerAddress;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(connectionAddress + "/api/employees/" + employeeId);

            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string result;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            if (result.Length == 0)
                return false;

            return true;
        }
    }
}