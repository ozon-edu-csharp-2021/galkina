using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests.MerchPackAggregateTests
{
    public class MerchTypeTests
    {
        [Theory]
        [MemberData(nameof(GetParseTestCases))]
        public void Parse_Success(int merchPackIndex, MerchType expected)
        {
            MerchType result = MerchType.Parse(merchPackIndex);
            
            Assert.Equal(expected, result);
        }
        
        public static IEnumerable<object[]> GetParseTestCases()
        {
            yield return new object[] { 10, MerchType.Welcome };
            yield return new object[] { 20, MerchType.ConferenceListener };
            yield return new object[] { 30, MerchType.ConferenceSpeaker };
            yield return new object[] { 40, MerchType.Starter };
            yield return new object[] { 50, MerchType.Veteran };
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(9)]
        [InlineData(-1)]
        [InlineData(11)]
        [InlineData(100)]
        public void Parse_MerchPackIndex_ThrowsException(int merchPackIndex)
        {
            Action act = () => MerchType.Parse(merchPackIndex);
            ArgumentException exception = Assert.Throws<ArgumentException>(act);
            
            Assert.Equal($"Unknown merch pack type {merchPackIndex}.", exception.Message);
        }
        
        [Theory]
        [MemberData(nameof(GetParseToIntTestCases))]
        public void ParseToInt_Success(MerchType merchType, int expected)
        {
            int result = merchType.ParseToInt();
            
            Assert.Equal(expected, result);
        }
        
        public static IEnumerable<object[]> GetParseToIntTestCases()
        {
            yield return new object[] { MerchType.Welcome, 10 };
            yield return new object[] { MerchType.ConferenceListener, 20 };
            yield return new object[] { MerchType.ConferenceSpeaker, 30 };
            yield return new object[] { MerchType.Starter, 40 };
            yield return new object[] { MerchType.Veteran, 50 };
        }
        
        [Theory]
        [MemberData(nameof(GetItemsTestCases))]
        public void Items_Success(MerchType merchType, List<ItemType> expected)
        {
            List<ItemType> result = merchType.Items;
            
            Assert.Equal(expected, result);
        }
        
        public static IEnumerable<object[]> GetItemsTestCases()
        {
            yield return new object[]
            {
                MerchType.Welcome,
                new List<ItemType>()
                {
                    ItemType.Pen, 
                    ItemType.Notepad
                }
            };
            yield return new object[]
            {
                MerchType.ConferenceListener,
                new List<ItemType>()
                {
                    ItemType.Pen, 
                    ItemType.Notepad
                }
            };
            yield return new object[]
            {
                MerchType.ConferenceSpeaker,
                new List<ItemType>()
                {
                    ItemType.TShirt,
                    ItemType.Pen,
                    ItemType.Notepad
                }
            };
            yield return new object[]
            {
                MerchType.Starter,
                new List<ItemType>()
                {
                    ItemType.TShirt,
                    ItemType.Sweatshirt
                }
            };
            yield return new object[]
            {
                MerchType.Veteran,
                new List<ItemType>()
                {
                    ItemType.Bag,
                    ItemType.Socks
                }
            };
        }
    }
}