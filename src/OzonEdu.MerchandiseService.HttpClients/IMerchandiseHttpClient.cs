using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.HttpModels;
using OzonEdu.MerchandiseService.Models;

namespace OzonEdu.MerchandiseService.HttpClients
{
    public interface IMerchandiseHttpClient
    {
        Task<bool> RequestMerchSet(int merchPackIndex, int size, CancellationToken token);
        Task<List<MerchSetResponse>> RetrieveIssuedMerchSetsInformation(int employeeId, CancellationToken token);
    }
}