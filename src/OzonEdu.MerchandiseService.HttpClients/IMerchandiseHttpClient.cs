using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.HttpModels;

namespace OzonEdu.MerchandiseService.HttpClients
{
    public interface IMerchandiseHttpClient
    {
        Task<MerchPackResponse> QueryMerchSet(long employeeId, int merchPackIndex, string size, CancellationToken token);
        Task<List<MerchPackResponse>> RetrieveIssuedMerchSetsInformation(long employeeId, CancellationToken token);
    }
}