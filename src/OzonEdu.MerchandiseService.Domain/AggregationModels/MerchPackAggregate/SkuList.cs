using System.Collections;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class SkuList : ValueObject, IEnumerable<Sku>
    {
        private List<Sku> skues;
        
        public List<Sku> Skues
        {
            get => skues;
        }

        public SkuList(MerchType merchType, ClothingSize size)
        {
            skues = new List<Sku>();
            FillScuList(merchType, size);
        }
        
        public IEnumerator<Sku> GetEnumerator()
        {
            return skues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach(Sku sku in skues)
                yield return sku;
        }

        private void FillScuList(MerchType merchType, ClothingSize size)
        {
            foreach (ItemType item in merchType.Items)
            {
                if (item == ItemType.TShirt)
                {
                    var skuTShirt = CalculateTShirtSize(size);
                    skues.Add(skuTShirt);
                }
                else if (item == ItemType.Sweatshirt)
                {
                    var skuSweatshirt = CalculateSweatshirtSize(size);
                    skues.Add(skuSweatshirt);
                }
                else if (item == ItemType.Bag)
                {
                    skues.Add(Sku.Bag);
                }
                else if (item == ItemType.Pen)
                {
                    skues.Add(Sku.Pen);
                }
                else if (item == ItemType.Notepad)
                {
                    skues.Add(Sku.Notepad);
                }
                else if (item == ItemType.Socks)
                {
                    skues.Add(Sku.Socks);
                }
            }
        }

        private Sku CalculateTShirtSize(ClothingSize size)
        {
            if(size == ClothingSize.XS)
                return Sku.TShirtXS;
            if(size == ClothingSize.S)
                return Sku.TShirtS;
            if(size == ClothingSize.M)
                return Sku.TShirtM;
            if(size == ClothingSize.L)
                return Sku.TShirtL;
            if(size == ClothingSize.XL)
                return Sku.TShirtXL;
            return Sku.TShirtXXL;
        }
        
        private Sku CalculateSweatshirtSize(ClothingSize size)
        {
            if(size == ClothingSize.XS)
                return Sku.SweatshirtXS;
            if(size == ClothingSize.S)
                return Sku.SweatshirtS;
            if(size == ClothingSize.M)
                return Sku.SweatshirtM;
            if(size == ClothingSize.L)
                return Sku.SweatshirtL;
            if(size == ClothingSize.XL)
                return Sku.SweatshirtXL;
            return Sku.SweatshirtXXL;
        }
    }
}