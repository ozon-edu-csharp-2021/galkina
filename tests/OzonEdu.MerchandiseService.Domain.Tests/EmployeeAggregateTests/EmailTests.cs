using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests.EmployeeAggregateTests
{
    public class EmailTests
    {
        [Theory]
        [InlineData("cat@mail.ru")]
        [InlineData("qwerty@gmail.com")]
        [InlineData("do12do@mail.ua")]
        [InlineData("ivan-ivanov@mail.de")]
        [InlineData("mail.mail@mail.com")]
        public void Create_Success(string validEmail)
        {
            Email result = Email.Create(validEmail);

            Assert.NotNull(result);
        }
        
        [Fact]
        public void Value_Success()
        {
            string expected = "cat@mail.com";

            Email email = Email.Create(expected);
            string result = email.Value;

            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData("@mail.com")]
        [InlineData("mail.com")]
        [InlineData("mail@mail")]
        [InlineData("@mail.")]
        [InlineData("mail")]
        [InlineData("@.")]
        [InlineData("@")]
        [InlineData(".")]
        [InlineData("")]
        [InlineData(null)]
        public void Create_InvalidEmail_ThrowsException(string invalidEmail)
        {
            Action act = () => Email.Create(invalidEmail);

            EmailInvalidException exception = Assert.Throws<EmailInvalidException>(act);
            Assert.Equal($"Email is invalid: {invalidEmail}", exception.Message);
        }
        
        [Fact]
        public void ToString_Success()
        {
            string expected = "cat@mail.com";

            Email email = Email.Create(expected);
            string result = email.ToString();

            Assert.Equal(expected, result);
        }
    }
}