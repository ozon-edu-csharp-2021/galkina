using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Queries;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Aggregate
{
    public class GetIssuedMerchPacksQueryHandler : IRequestHandler<GetIssuedMerchPacksQuery, List<MerchPack>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMerchPackRepository _merchPackRepository;

        public GetIssuedMerchPacksQueryHandler(IMerchPackRepository merchPackRepository, IUnitOfWork unitOfWork)
        {
            _merchPackRepository = merchPackRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<List<MerchPack>> Handle(GetIssuedMerchPacksQuery request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);
            
            List<MerchPack> merchPacks = await _merchPackRepository.GetIssuedMerchPacksToEmployeeAsync(request.EmployeeId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return merchPacks;
        }
    }
}