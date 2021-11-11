using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class ItemType : Enumeration
    {
        public static ItemType TShirt = new(1, nameof(TShirt));
        public static ItemType Sweatshirt = new(2, nameof(Sweatshirt));
        public static ItemType Notepad = new(3, nameof(Notepad));
        public static ItemType Bag = new(4, nameof(Bag));
        public static ItemType Pen = new(5, nameof(Pen));
        public static ItemType Socks = new(6, nameof(Socks));

        private ItemType(int id, string name) : base(id, name)
        {
        }
    }
}