using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.DomainServices;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Stub;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Aggregate
{
    public class GiveMerchPackAtEmployeeRequestCommandHandler : IRequestHandler<GiveMerchPackAtEmployeeRequestCommand, MerchPack>
    {
        private readonly IMerchPackRepository _merchPackRepository;

        public GiveMerchPackAtEmployeeRequestCommandHandler(IMerchPackRepository merchPackRepository)
        {
            _merchPackRepository = merchPackRepository;
        }
        
        public async Task<MerchPack> Handle(GiveMerchPackAtEmployeeRequestCommand request, CancellationToken cancellationToken)
        {
            //TODO:
            List<Employee> employees = EmployeesServiceStub.GetAll();
            Employee employee = DomainService.DoesEmployeeExist(employees, request.EmployeeId);
            if (employee == null)
            {
                throw new Exception($"Employee with Id {request.EmployeeId} not found.");
            }
            
            MerchPack merchPack = new MerchPack(request.Type, request.ClothingSize, employee);
            
            List<MerchPack> merchPacksInDb = await _merchPackRepository.GetIssuedMerchPacksToEmployeeAsync(request.EmployeeId, cancellationToken);
            if (DomainService.DidMerchPackIssue(merchPacksInDb, request.Type))
            {
                merchPack.Cancel();
                throw new Exception($"{request.Type} already issued in this year.");
            }
            
            merchPack.Validate();
            
            bool isScuAvailable = true;
            foreach(Sku sku in merchPack.Items)
            {
                // TODO: isScuAvailable = StockAPI.DoesSkuAvailable(sku);
                if (!isScuAvailable)
                {
                    merchPack.StockAwaitDelivery();
                    break;
                }
            }

            if (isScuAvailable)
            {
                merchPack.StockConfirm();
                // TODO:StockAPI.ReserveScues(merchPack.Items);
                merchPack.StockReserve();
            }

            long id = await _merchPackRepository.AddMerchPackAsync(merchPack, cancellationToken);
            await _merchPackRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            
            return merchPack;
        }
    }
}