using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Models;

namespace OzonEdu.MerchandiseService.Services.Interfaces
{
    public interface IMerchandiseService
    {
        Task<MerchSet> QueryMerchSet(int merchPackIndex, int size, CancellationToken token);

        Task<List<MerchSet>> RetrieveIssuedMerchSetsInformation(int employeeId, CancellationToken token);
    }
}