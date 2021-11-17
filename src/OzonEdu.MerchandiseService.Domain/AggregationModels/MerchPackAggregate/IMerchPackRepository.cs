using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public interface IMerchPackRepository: IRepository<MerchPack>
    {
        Task<long> AddMerchPackAsync(MerchPack merchPack, CancellationToken token);

        Task<long> UpdateMerchPack(MerchPack merchPack, CancellationToken token);

        Task<List<MerchPack>> GetIssuedMerchPacksToEmployeeAsync(long employeeId, CancellationToken cancellationToken = default);
        
        Task<List<MerchPack>> GetMerchPacksAwaitedDeliveryAsync(long employeeId, CancellationToken cancellationToken = default);
    }
}