using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using OzonEdu.MerchandiseService.Services.Interfaces;

namespace OzonEdu.MerchandiseService.GrpcServices
{
    public class MerchandiseServiceGrpcService : MerchandiseServiceGrpc.MerchandiseServiceGrpcBase
    {
        private readonly IMerchandiseService _merchandiseService;

        public MerchandiseServiceGrpcService(IMerchandiseService merchandiseService)
        {
            _merchandiseService = merchandiseService;
        }

        public override async Task<QueryMerchSetResponse> QueryMerchSet(QueryMerchSetRequest request, ServerCallContext context)
        {
            var merchSet = await _merchandiseService.QueryMerchSet(request.MerchPackIndex, request.Size, context.CancellationToken);

            return new QueryMerchSetResponse()
            {
                MerchSetId = merchSet.MerchSetId,
                MerchPack = merchSet.MerchPack,
                Skues = { merchSet.Skues.Select(i => new Sku
                {
                    SkuId = i.SkuId,
                    SkuName = i.SkuName,
                    Size = i.Size
                })  }
            };
        }

        public override async Task<RetrieveIssuedMerchSetsInformationResponse> RetrieveIssuedMerchSetsInformation
            (RetrieveIssuedMerchSetsInformationRequest request, ServerCallContext context)
        {
            var merchSets = await _merchandiseService.RetrieveIssuedMerchSetsInformation(request.EmployeeId, context.CancellationToken);
            
            return new RetrieveIssuedMerchSetsInformationResponse
            {
                MerchSets = { merchSets.Select(x => new MerchSet
                {
                    MerchSetId = x.MerchSetId,
                    MerchPack = x.MerchPack,
                    Skues = { x.Skues.Select(i => new Sku
                    {
                        SkuId = i.SkuId,
                        SkuName = i.SkuName,
                        Size = i.Size
                    })  }
                })}
            };
        }
    }
}