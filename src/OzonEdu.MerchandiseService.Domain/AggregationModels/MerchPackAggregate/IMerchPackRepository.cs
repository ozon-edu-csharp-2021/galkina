using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public interface IMerchPackRepository: IRepository<MerchPack>
    {
        Task<MerchPack> AddMerchPackAsync(MerchPack merchPack, CancellationToken token);

        Task<MerchPack> UpdateMerchPackAsync(MerchPack merchPack, CancellationToken token);

        Task<List<MerchPack>> GetIssuedMerchPacksToEmployeeAsync(long employeeId, CancellationToken cancellationToken);
        
        Task<List<MerchPack>> GetMerchPacksAwaitedDeliveryAsync(CancellationToken cancellationToken);
    }
}