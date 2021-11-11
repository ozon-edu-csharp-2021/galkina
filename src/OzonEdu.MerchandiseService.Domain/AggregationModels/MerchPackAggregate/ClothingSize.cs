using System;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class ClothingSize : Enumeration
    {
        public static ClothingSize XS = new(42, nameof(XS));
        public static ClothingSize S = new(44, nameof(S));
        public static ClothingSize M = new(46, nameof(M));
        public static ClothingSize L = new(48, nameof(L));
        public static ClothingSize XL = new(50, nameof(XL));
        public static ClothingSize XXL = new(52, nameof(XXL));

        private ClothingSize(int id, string name) : base(id, name)
        {
        }
        
        public static ClothingSize Parse(string size)
            => size?.ToUpper() switch
            {
                "XS"  => XS,
                "S"   => S,
                "M"   => M,
                "L"   => L,
                "XL"  => XL,
                "XXL" => XXL,
                _ => throw new ArgumentException($"Unknown size {size}.")
            };

        public int ParseToInt()
            => this.Id;
    }
}