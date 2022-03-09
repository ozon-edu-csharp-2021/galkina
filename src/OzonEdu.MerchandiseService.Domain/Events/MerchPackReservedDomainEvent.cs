using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public class MerchPackReservedDomainEvent : INotification
    {
        public long EmployeeId { get; }
        public string EmployeeEmail { get; }
        
        public string EmployeeName { get; }
        
        public MerchType MerchType { get; }
        
        public ClothingSize ClothingSize { get; }

        public MerchPackReservedDomainEvent(long id, string email, string name, MerchType merchType, ClothingSize clothingSize)
        {
            EmployeeId = id;
            EmployeeEmail = email;
            EmployeeName = name;
            MerchType = merchType;
            ClothingSize = clothingSize;
        }
    }
}