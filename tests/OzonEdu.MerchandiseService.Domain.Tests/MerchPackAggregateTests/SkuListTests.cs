using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests.MerchPackAggregateTests
{
    public class SkuListTests
    {
        [Fact]
        public void Ctor_Success()
        {
            SkuList result = new SkuList(MerchType.Welcome, ClothingSize.S);
            
            Assert.NotNull(result);
        }

        [Theory]
        [MemberData(nameof(GetSkuesTestCases))]
        public void Skues_Success(MerchType merchType, ClothingSize size, List<Sku> expected)
        {
            SkuList skuList = new SkuList(merchType, size);
            
            List<Sku> result = skuList.Skues;
            
            Assert.Equal(expected, result);
        }
        
        public static IEnumerable<object[]> GetSkuesTestCases()
        {
            yield return new object[]
            {
                MerchType.Welcome, ClothingSize.XS,
                new List<Sku>()
                {
                    Sku.Pen, 
                    Sku.Notepad
                }
            };
            yield return new object[]
            {
                MerchType.ConferenceListener, ClothingSize.S,
                new List<Sku>()
                {
                    Sku.Pen, 
                    Sku.Notepad
                }
            };
            yield return new object[]
            {
                MerchType.ConferenceSpeaker, ClothingSize.M,
                new List<Sku>()
                {
                    Sku.TShirtM,
                    Sku.Pen,
                    Sku.Notepad
                }
            };
            yield return new object[]
            {
                MerchType.Starter, ClothingSize.L,
                new List<Sku>()
                {
                    Sku.TShirtL,
                    Sku.SweatshirtL
                }
            };
            yield return new object[]
            {
                MerchType.Veteran, ClothingSize.XL,
                new List<Sku>()
                {
                    Sku.Bag,
                    Sku.Socks
                }
            };
        }
    }
}