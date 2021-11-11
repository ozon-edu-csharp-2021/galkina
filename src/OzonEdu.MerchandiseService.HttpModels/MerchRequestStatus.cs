namespace OzonEdu.MerchandiseService.HttpModels
{
    public enum MerchRequestStatus
    {
        Submitted = 1,
        Validated = 2,
        StockConfirmed = 3,
        StockAwaitedDelivery = 4,
        StockReserved = 5,
        Cancelled = 6
    }
}