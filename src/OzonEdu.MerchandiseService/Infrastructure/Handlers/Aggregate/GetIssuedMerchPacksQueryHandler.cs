using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Aggregate
{
    public class GetIssuedMerchPacksQueryHandler : IRequestHandler<GetIssuedMerchPacksQuery, List<MerchPack>>
    {
        private readonly IMerchPackRepository _merchPackRepository;

        public GetIssuedMerchPacksQueryHandler(IMerchPackRepository merchPackRepository)
        {
            _merchPackRepository = merchPackRepository;
        }
        
        public async Task<List<MerchPack>> Handle(GetIssuedMerchPacksQuery request, CancellationToken cancellationToken)
        {
            List<MerchPack> merchPacksInDb = await _merchPackRepository.GetIssuedMerchPacksToEmployeeAsync(request.EmployeeId, cancellationToken);

            return merchPacksInDb;
        }
    }
}