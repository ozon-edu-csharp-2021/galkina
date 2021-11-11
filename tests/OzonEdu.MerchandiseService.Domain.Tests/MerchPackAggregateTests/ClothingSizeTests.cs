using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests.MerchPackAggregateTests
{
    public class ClothingSizeTests
    {
        [Theory]
        [MemberData(nameof(GetParseTestCases))]
        public void Parse_Success(string size, ClothingSize expected)
        {
            ClothingSize result = ClothingSize.Parse(size);
            
            Assert.Equal(expected, result);
        }
        
        public static IEnumerable<object[]> GetParseTestCases()
        {
            yield return new object[] { "xs", ClothingSize.XS };
            yield return new object[] { "s", ClothingSize.S };
            yield return new object[] { "m", ClothingSize.M };
            yield return new object[] { "L", ClothingSize.L };
            yield return new object[] { "XL", ClothingSize.XL };
            yield return new object[] { "XXL", ClothingSize.XXL };
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("xxs")]
        [InlineData("44")]
        [InlineData("large")]
        public void Parse_InvalidSize_ThrowsException(string size)
        {
            Action act = () => ClothingSize.Parse(size);
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            
            Assert.Equal($"Unknown size {size}.", exception.Message);
        }
        
        [Theory]
        [MemberData(nameof(GetParseToIntTestCases))]
        public void ParseToInt_Success(ClothingSize clothingSize, int expected)
        {
            int result = clothingSize.ParseToInt();
            
            Assert.Equal(expected, result);
        }
        
        public static IEnumerable<object[]> GetParseToIntTestCases()
        {
            yield return new object[] { ClothingSize.XS, 42 };
            yield return new object[] { ClothingSize.S, 44 };
            yield return new object[] { ClothingSize.M, 46 };
            yield return new object[] { ClothingSize.L, 48 };
            yield return new object[] { ClothingSize.XL, 50 };
            yield return new object[] { ClothingSize.XXL, 52 };
        }
    }
}