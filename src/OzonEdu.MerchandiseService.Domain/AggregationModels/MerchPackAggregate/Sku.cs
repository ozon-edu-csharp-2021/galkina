using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class Sku : Enumeration
    {
        public static Sku Pen = new(1, nameof(Pen));
        public static Sku Notepad = new(2, nameof(Notepad));
        public static Sku CardHolder = new(3, nameof(CardHolder));
        public static Sku Socks = new(4, nameof(Socks));
        public static Sku TShirtXS = new(5, nameof(TShirtXS));
        public static Sku TShirtS = new(6, nameof(TShirtS));
        public static Sku TShirtM = new(7, nameof(TShirtM));
        public static Sku TShirtL = new(8, nameof(TShirtL));
        public static Sku TShirtXL = new(9, nameof(TShirtXL));
        public static Sku TShirtXXL = new(10, nameof(TShirtXXL));
        public static Sku SweatshirtXS = new(11, nameof(SweatshirtXS));
        public static Sku SweatshirtS = new(12, nameof(SweatshirtS));
        public static Sku SweatshirtM = new(13, nameof(SweatshirtM));
        public static Sku SweatshirtL = new(14, nameof(SweatshirtL));
        public static Sku SweatshirtXL = new(15, nameof(SweatshirtXL));
        public static Sku SweatshirtXXL = new(16, nameof(SweatshirtXXL));
        
        public Sku(int id, string name) : base(id, name)
        {
        }
    }
}