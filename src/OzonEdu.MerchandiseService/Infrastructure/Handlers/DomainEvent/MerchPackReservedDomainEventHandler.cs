using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.Events;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.DomainEvent
{
    public class MerchPackReservedDomainEventHandler : INotificationHandler<MerchPackReservedDomainEvent> 
    {
        public Task Handle(MerchPackReservedDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}