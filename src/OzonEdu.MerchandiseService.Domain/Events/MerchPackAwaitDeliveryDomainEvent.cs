using MediatR;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public class MerchPackAwaitDeliveryDomainEvent : INotification
    {
        public long EmployeeId { get; }
        public string EmployeeEmail { get; }
        public string EmployeeName { get; }

        public MerchPackAwaitDeliveryDomainEvent(long id, string email, string name)
        {
            EmployeeId = id;
            EmployeeEmail = email;
            EmployeeName = name;
        }
    }
}