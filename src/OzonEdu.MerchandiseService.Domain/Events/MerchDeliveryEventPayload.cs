using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public class MerchDeliveryEventPayload
    {
        public MerchType MerchType { get; }

        public MerchDeliveryEventPayload(MerchType merchType)
        {
            MerchType = merchType;
        }
    }
}