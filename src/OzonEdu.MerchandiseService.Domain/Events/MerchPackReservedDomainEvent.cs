using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public class MerchPackReservedDomainEvent : INotification
    {
        public string EmployeeEmail { get; }
        
        public string EmployeeName { get; }
        
        public EmployeeEventType EventType { get; }
        
        public object? Payload { get; }

        public MerchPackReservedDomainEvent(string email, string name, MerchType merchType)
        {
            EmployeeEmail = email;
            EmployeeName = name;
            EventType = EmployeeEventType.MerchDelivery;
            Payload = new MerchDeliveryEventPayload(merchType);
        }
    }
}