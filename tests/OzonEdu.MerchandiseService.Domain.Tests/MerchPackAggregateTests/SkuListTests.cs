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
    }
}