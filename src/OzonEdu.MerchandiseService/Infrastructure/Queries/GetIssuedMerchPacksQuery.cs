using System.Collections.Generic;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.Queries
{
    public class GetIssuedMerchPacksQuery : IRequest<List<MerchPack>>
    {
        public long EmployeeId { get; set; }
    }
}