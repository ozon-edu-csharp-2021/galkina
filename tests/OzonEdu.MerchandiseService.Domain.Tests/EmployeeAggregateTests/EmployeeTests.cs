using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests.EmployeeAggregateTests
{
    public class EmployeeTests
    {
        [Theory]
        [InlineData(1, "Иван", "Иванович", "Иванов", "ivan@mail.ru")]
        [InlineData(1111111111111111, "Мария", "Васильевна", "Иванова", "marya@mail.ru")]
        public void Ctor_Success(long id, string firstName, string middleName, string lastName, string email)
        {
            Employee result = new Employee(id, firstName, middleName, lastName, email);

            Assert.NotNull(result);
        }
        
        [Fact]
        public void FirstName_Success()
        {
            string expected = "Иван";
            
            Employee employee = new Employee(1, expected, "Иванович", "Иванов", "ivan@mail.ru");
            string result = employee.FirstName.Value;
            
            Assert.Equal(expected,result);
        }
        
        [Fact]
        public void MiddleName_Success()
        {
            string expected = "Иванович";
            
            Employee employee = new Employee(1, "Иван", expected, "Иванов", "ivan@mail.ru");
            string result = employee.MiddleName.Value;
            
            Assert.Equal(expected,result);
        }
        
        [Fact]
        public void LastName_Success()
        {
            string expected = "Иванов";
            
            Employee employee = new Employee(1, "Иван", "Иванович", expected, "ivan@mail.ru");
            string result = employee.LastName.Value;
            
            Assert.Equal(expected,result);
        }
        
        [Fact]
        public void Email_Success()
        {
            string expected = "ivan@mail.ru";
            
            Employee employee = new Employee(1, "Иван", "Иванович", "Иванов", expected);
            string result = employee.Email.Value;
            
            Assert.Equal(expected,result);
        }
        
        [Fact]
        public void ToString_Success()
        {
            string expected = "Иван Иванович Иванов";
            
            Employee employee = new Employee(1, "Иван", "Иванович", "Иванов", "ivan@mail.ru");
            string result = employee.ToString();
            
            Assert.Equal(expected,result);
        }
    }
}