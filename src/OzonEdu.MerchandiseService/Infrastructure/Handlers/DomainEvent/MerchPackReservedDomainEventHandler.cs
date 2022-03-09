using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using enums = CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Infrastructure.MessageBroker;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.DomainEvent
{
    public class MerchPackReservedDomainEventHandler : INotificationHandler<MerchPackReservedDomainEvent> 
    {
        private readonly IProducerBuilderWrapper _producerBuilderWrapper;

        public MerchPackReservedDomainEventHandler(IProducerBuilderWrapper producerBuilderWrapper)
        {
            _producerBuilderWrapper = producerBuilderWrapper;
        }

        public Task Handle(MerchPackReservedDomainEvent notification, CancellationToken cancellationToken)
        {
            _producerBuilderWrapper.Producer.Produce(_producerBuilderWrapper.EmployeeNotificationTopic,
                new Message<string, string>()
                {
                    Key = notification.EmployeeId.ToString(),
                    Value = JsonSerializer.Serialize(new NotificationEvent()
                    {
                        EmployeeEmail = notification.EmployeeEmail,
                        EmployeeName = notification.EmployeeName,
                        EventType = EmployeeEventType.MerchDelivery,
                        Payload = new MerchDeliveryEventPayload()
                        {
                            MerchType = (enums.MerchType)notification.MerchType.ParseToInt(),
                            ClothingSize = (enums.ClothingSize)notification.ClothingSize.ParseToInt()
                        }
                    })
                });
            
            return Task.CompletedTask;
        }
    }
}