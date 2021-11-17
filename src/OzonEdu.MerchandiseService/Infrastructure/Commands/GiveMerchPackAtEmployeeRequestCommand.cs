using System;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{
    public class GiveMerchPackAtEmployeeRequestCommand : IRequest<MerchPack>
    {
        public MerchType Type { get; set; }
        public ClothingSize ClothingSize { get; set; }
        public long EmployeeId { get; set; }
    }
}