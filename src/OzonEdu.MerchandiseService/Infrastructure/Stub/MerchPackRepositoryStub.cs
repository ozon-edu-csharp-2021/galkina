using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.Stub
{
    public class MerchPackRepositoryStub : IMerchPackRepository
    {
        private IUnitOfWork _unitOfWork;
        
        private List<MerchPack> merchPacks = new List<MerchPack>()
        {
            new MerchPack(MerchType.Welcome, ClothingSize.M, new Employee(1, "Василий", "Васильевич", "Васечкин", "vasya@mail.ru")),
            new MerchPack(MerchType.Starter, ClothingSize.M, new Employee(1, "Василий", "Васильевич", "Васечкин", "vasya@mail.ru")),
            new MerchPack(MerchType.Veteran, ClothingSize.XS, new Employee(2, "Иван", "Васильевич", "Иванов", "van@mail.ru"))
        };
        
        public IUnitOfWork UnitOfWork { get => _unitOfWork; }

        public MerchPackRepositoryStub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public Task<long> AddMerchPackAsync(MerchPack merchPack, CancellationToken token)
        {
            return Task.FromResult((long)1);
        }

        public Task<long> UpdateMerchPack(MerchPack merchPack, CancellationToken token)
        {
            return Task.FromResult((long)2);
        }

        public Task<List<MerchPack>> GetIssuedMerchPacksToEmployeeAsync(long employeeId, CancellationToken cancellationToken = default)
        {
            var result = merchPacks.Where(m => m.Employee.Id == employeeId).ToList();
            return Task.FromResult(result);
        }

        public Task<List<MerchPack>> GetMerchPacksAwaitedDeliveryAsync(long employeeId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(merchPacks);
        }
    }
}