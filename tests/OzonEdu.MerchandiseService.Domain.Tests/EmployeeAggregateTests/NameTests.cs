using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests.EmployeeAggregateTests
{
    public class NameTests
    {
        [Theory]
        [InlineData("Ия")]
        [InlineData("Максим")]
        [InlineData("Марьян-Станислав")]
        [InlineData("Сassandra")]
        [InlineData("Иванов")]
        [InlineData("Тригорский-Радзинский")]
        [InlineData("Brown")]
        [InlineData("Григорьевич")]
        [InlineData("Алексеевна")]
        public void Create_Success(string validName)
        {
            Name result = Name.Create(validName);

            Assert.NotNull(result);
        }
        
        [Fact]
        public void Value_Success()
        {
            string expected = "Ирина";

            Name name = Name.Create(expected);
            string result = name.Value;

            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData("А")]
        [InlineData("123")]
        [InlineData("А12а")]
        [InlineData("А-А")]
        [InlineData("Анна-Мария-Изольда")]
        [InlineData("Ааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааааа")]
        [InlineData("")]
        [InlineData(null)]
        public void Create_InvalidName_ThrowsException(string invalidName)
        {
            Action act = () => Name.Create(invalidName);

            NameInvalidException exception = Assert.Throws<NameInvalidException>(act);
            Assert.Equal($"Name is invalid: {invalidName}", exception.Message);
        }
        
        [Fact]
        public void ToString_Success()
        {
            string expected = "Николай";

            Name name = Name.Create(expected);
            string result = name.ToString();

            Assert.Equal(expected, result);
        }
    }
}