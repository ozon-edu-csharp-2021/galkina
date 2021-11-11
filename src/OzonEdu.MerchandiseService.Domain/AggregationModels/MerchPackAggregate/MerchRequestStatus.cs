using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchRequestStatus: Enumeration
    {
        public static MerchRequestStatus Submitted = new(1, nameof(Submitted));
        public static MerchRequestStatus Validated = new(2, nameof(Validated));
        public static MerchRequestStatus StockConfirmed = new(3, nameof(StockConfirmed));
        public static MerchRequestStatus StockAwaitedDelivery = new(4, nameof(StockAwaitedDelivery));
        public static MerchRequestStatus StockReserved = new(5, nameof(StockReserved));
        public static MerchRequestStatus Cancelled = new(6, nameof(Cancelled));
        
        private MerchRequestStatus(int id, string name) : base(id, name)
        {
        }
        
        public int ParseToInt()
            => this.Id;
    }
}