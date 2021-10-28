using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
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

        public override async Task<BoolValue> RequestMerchSet(RequestMerchSetRequest request, ServerCallContext context)
        {
            bool isMerchRequested = await _merchandiseService.RequestMerchSet(request.MerchPackIndex, request.Size, context.CancellationToken);

            return new BoolValue() { Value = isMerchRequested };
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