using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.DomainServices;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Stub;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Aggregate
{
    public class GiveMerchPackAtEmployeeRequestCommandHandler : IRequestHandler<GiveMerchPackAtEmployeeRequestCommand, MerchPack>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMerchPackRepository _merchPackRepository;

        public GiveMerchPackAtEmployeeRequestCommandHandler(IMerchPackRepository merchPackRepository, IUnitOfWork unitOfWork)
        {
            _merchPackRepository = merchPackRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<MerchPack> Handle(GiveMerchPackAtEmployeeRequestCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);
            //TODO:
            List<Employee> employees = EmployeesServiceStub.GetAll();
            Employee employee;
            if (!DomainService.TryGetEmployee(employees, request.EmployeeId, out employee))
            {
                throw new Exception($"Employee with Id {request.EmployeeId} not found.");
            }
            
            MerchPack merchPack = new MerchPack(request.Type, request.ClothingSize, employee);
            
            List<MerchPack> merchPacksInDb = await _merchPackRepository.GetIssuedMerchPacksToEmployeeAsync(request.EmployeeId, cancellationToken);
            if (DidMerchPackIssue(merchPacksInDb, request.Type))
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

            var merchpack = await _merchPackRepository.AddMerchPackAsync(merchPack, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return merchPack;
        }
        
        public bool DidMerchPackIssue(List<MerchPack> merchPacks, MerchType type)
        {
            MerchPack merchPack = merchPacks.Where(m => m.Type.Id == type.Id)

            if (merchPack == null)
                return false;

            TimeSpan oneYear = new TimeSpan(365, 0, 0, 0);

            return DateTime.Now - merchPack.IssueDate < oneYear;
        }
    }
}