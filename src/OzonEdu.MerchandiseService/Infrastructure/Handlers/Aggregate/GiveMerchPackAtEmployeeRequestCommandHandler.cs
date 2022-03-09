using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Configuration;
using OzonEdu.MerchandiseService.Infrastructure.Handlers.Aggregate.RequestsToStockApi;
using OzonEdu.StockApi.Grpc;
using OzonEdu.MerchandiseService.Infrastructure.Models;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Aggregate
{
    public class GiveMerchPackAtEmployeeRequestCommandHandler : IRequestHandler<GiveMerchPackAtEmployeeRequestCommand, MerchPack>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly StockApiGrpc.StockApiGrpcClient _stockServiceClient;
        private static IConfiguration _configuration;

        public GiveMerchPackAtEmployeeRequestCommandHandler(IMerchPackRepository merchPackRepository, IUnitOfWork unitOfWork, 
            StockApiGrpc.StockApiGrpcClient stockServiceClient, IConfiguration configuration)
        {
            _merchPackRepository = merchPackRepository;
            _unitOfWork = unitOfWork;
            _stockServiceClient = stockServiceClient;
            _configuration = configuration;
        }

        public async Task<MerchPack> Handle(GiveMerchPackAtEmployeeRequestCommand request,
            CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);

            if (!TryGetEmployee(request.EmployeeId, out Employee employee))
                throw new Exception($"Employee with Id {request.EmployeeId} not found.");

            MerchPack merchPack = new MerchPack(request.Type, request.ClothingSize, employee);

            List<MerchPack> merchPacksInDb =
                await _merchPackRepository.GetIssuedMerchPacksToEmployeeAsync(request.EmployeeId, cancellationToken);
            if (DidMerchPackIssue(merchPacksInDb, request.Type))
            {
                merchPack.Cancel();
                throw new Exception($"{request.Type} already issued in this year.");
            }

            merchPack.Validate();
            bool isSkusAvailable = await RequestToStockApi.DoesSkusAvailable(_stockServiceClient, merchPack.Items.Skues, cancellationToken);
            
            if (!isSkusAvailable)
            {
                merchPack.StockAwaitDelivery();
            }

            if (isSkusAvailable)
            {
                merchPack.StockConfirm();
                if (await RequestToStockApi.ReserveSkus(_stockServiceClient, merchPack.Items.Skues, cancellationToken))
                    merchPack.StockReserve();
                else
                    merchPack.StockAwaitDelivery();
            }

            await _merchPackRepository.AddMerchPackAsync(merchPack, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return merchPack;
        }

        private bool DidMerchPackIssue(List<MerchPack> merchPacks, MerchType type)
        {
            MerchPack merchPack = merchPacks.Where(m => m.Type.Id == type.Id)
                .OrderByDescending(m => m.IssueDate)
                .FirstOrDefault();

            if (merchPack == null)
                return false;

            TimeSpan oneYear = new TimeSpan(365, 0, 0, 0);

            return DateTime.Now - merchPack.IssueDate < oneYear;
        }
        
        private static bool TryGetEmployee(long employeeId, out Employee employee)
        {
            employee = null;
            
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
            
            EmployeeDto emp = JsonSerializer.Deserialize<EmployeeDto>(result);
            
            if(emp is not null)
                employee = new Employee(emp.id, emp.firstName, emp.lastName, emp.middleName, emp.email);

            return true;
        }

       
    }
}