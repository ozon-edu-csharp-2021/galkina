using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests.MerchPackAggregateTests
{
    public class MerchRequestStatusTests
    {
        [Theory]
        [MemberData(nameof(GetParseToIntTestCases))]
        public void ParseToInt_Success(MerchRequestStatus merchRequestStatus, int expected)
        {
            int result = merchRequestStatus.ParseToInt();
            
            Assert.Equal(expected, result);
        }
        
        public static IEnumerable<object[]> GetParseToIntTestCases()
        {
            yield return new object[] { MerchRequestStatus.Submitted, 1 };
            yield return new object[] { MerchRequestStatus.Validated, 2 };
            yield return new object[] { MerchRequestStatus.StockConfirmed, 3 };
            yield return new object[] { MerchRequestStatus.StockAwaitedDelivery, 4 };
            yield return new object[] { MerchRequestStatus.StockReserved, 5 };
            yield return new object[] { MerchRequestStatus.Cancelled, 6 };
        }
    }
}