using System.Collections;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class SkuList : ValueObject
    {
        private List<long> skues;
        
        public List<long> Skues
        {
            get => skues;
        }

        public SkuList(MerchType merchType, ClothingSize size)
        {
            FillScuList(merchType, size);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach(long sku in skues)
                yield return sku;
        }

        private void FillScuList(MerchType merchType, ClothingSize size)
        {
            if (merchType == MerchType.Starter)
            {
                if (size == ClothingSize.XS)
                {
                    skues = new List<long> {1, 41, 42, 43};
                    return;
                }
                if (size == ClothingSize.S)
                {
                    skues = new List<long> {2, 41, 42, 43};
                    return;
                }
                if (size == ClothingSize.M)
                {
                    skues = new List<long> {3, 41, 42, 43};
                    return;
                }
                if (size == ClothingSize.L)
                {
                    skues = new List<long> {4, 41, 42, 43};
                    return;
                }
                if (size == ClothingSize.XL)
                {
                    skues = new List<long> {5, 41, 42, 43};
                    return;
                }
                if (size == ClothingSize.XXL)
                {
                    skues = new List<long> {6, 41, 42, 43};
                    return;
                }
            }
            if (merchType == MerchType.Welcome)
            {
                if (size == ClothingSize.XS)
                {
                    skues = new List<long> {7, 13};
                    return;
                }
                if (size == ClothingSize.S)
                {
                    skues = new List<long> {8, 14};
                    return;
                }
                if (size == ClothingSize.M)
                {
                    skues = new List<long> {9, 15};
                    return;
                }
                if (size == ClothingSize.L)
                {
                    skues = new List<long> {10, 16};
                    return;
                }
                if (size == ClothingSize.XL)
                {
                    skues = new List<long> {11, 17};
                    return;
                }
                if (size == ClothingSize.XXL)
                {
                    skues = new List<long> {12, 18};
                    return;
                }
            }
            if (merchType == MerchType.ConferenceListener)
            {
                if (size == ClothingSize.XS)
                {
                    skues = new List<long> {25, 46, 47};
                    return;
                }
                if (size == ClothingSize.S)
                {
                    skues = new List<long> {26, 46, 47};
                    return;
                }
                if (size == ClothingSize.M)
                {
                    skues = new List<long> {27, 46, 47};
                    return;
                }
                if (size == ClothingSize.L)
                {
                    skues = new List<long> {28, 46, 47};
                    return;
                }
                if (size == ClothingSize.XL)
                {
                    skues = new List<long> {29, 46, 47};
                    return;
                }
                if (size == ClothingSize.XXL)
                {
                    skues = new List<long> {30, 46, 47};
                    return;
                }
            }
            if (merchType == MerchType.ConferenceSpeaker)
            {
                if (size == ClothingSize.XS)
                {
                    skues = new List<long> {19, 44, 45};
                    return;
                }
                if (size == ClothingSize.S)
                {
                    skues = new List<long> {20, 44, 45};
                    return;
                }
                if (size == ClothingSize.M)
                {
                    skues = new List<long> {21, 44, 45};
                    return;
                }
                if (size == ClothingSize.L)
                {
                    skues = new List<long> {22, 44, 45};
                    return;
                }
                if (size == ClothingSize.XL)
                {
                    skues = new List<long> {23, 44, 45};
                    return;
                }
                if (size == ClothingSize.XXL)
                {
                    skues = new List<long> {24, 44, 45};
                    return;
                }
            }
            if (merchType == MerchType.Veteran)
            {
                if (size == ClothingSize.XS)
                {
                    skues = new List<long> {31, 36, 48, 49, 50};
                    return;
                }
                if (size == ClothingSize.S)
                {
                    skues = new List<long> {32, 37, 48, 49, 50};
                    return;
                }
                if (size == ClothingSize.M)
                {
                    skues = new List<long> {33, 38, 48, 49, 50};
                    return;
                }
                if (size == ClothingSize.L)
                {
                    skues = new List<long> {34, 39, 48, 49, 50};
                    return;
                }
                if (size == ClothingSize.XL)
                {
                    skues = new List<long> {35, 40, 48, 49, 50};
                    return;
                }
                if (size == ClothingSize.XXL)
                {
                    skues = new List<long> {36, 41, 48, 49, 50};
                    return;
                }
            }
        }
    }
}