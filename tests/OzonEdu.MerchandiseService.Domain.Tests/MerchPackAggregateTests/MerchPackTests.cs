using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests.MerchPackAggregateTests
{
    public class MerchPackTests
    {
        Employee employee;
        MerchPack merchPack;

        public MerchPackTests()
        {
            employee = new Employee(1, "Иван", "Иванович", "Иванов", "ivan@mail.ru");
            merchPack = new MerchPack(MerchType.Welcome, ClothingSize.XS, employee);
        }
        
        [Fact]
        public void Ctor_Success()
        {
            Assert.NotNull(merchPack);
        }

        [Fact]
        public void Type_Success()
        {
            MerchType expected = MerchType.Welcome;

            MerchType result = merchPack.Type;
            
            Assert.Equal(expected,result);
        }
        
        [Fact]
        public void ClothingSize_Success()
        {
            ClothingSize expected = ClothingSize.XS;

            ClothingSize result = merchPack.ClothingSize;
            
            Assert.Equal(expected,result);
        }
        
        [Fact]
        public void Items_Success()
        {
            SkuList expected = new SkuList(MerchType.Welcome, ClothingSize.XS);

            SkuList result = merchPack.Items;
            
            Assert.Equal(expected,result);
        }
        
        [Fact]
        public void Employee_Success()
        {
            Employee expected = employee;

            Employee result = merchPack.Employee;
            
            Assert.Equal(expected,result);
        }
        
        [Fact]
        public void Status_Success()
        {
            MerchRequestStatus result = merchPack.Status;
            Assert.Equal(MerchRequestStatus.Submitted,result);
            
            merchPack.Validate();
            result = merchPack.Status;
            Assert.Equal(MerchRequestStatus.Validated,result);
            
            merchPack.StockAwaitDelivery();
            result = merchPack.Status;
            Assert.Equal(MerchRequestStatus.StockAwaitedDelivery,result);
            
            merchPack.Validate();
            result = merchPack.Status;
            Assert.Equal(MerchRequestStatus.Validated,result);
            
            merchPack.StockConfirm();
            result = merchPack.Status;
            Assert.Equal(MerchRequestStatus.StockConfirmed,result);
            
            merchPack.StockReserve();
            result = merchPack.Status;
            Assert.Equal(MerchRequestStatus.StockReserved,result);
            
            merchPack.Cancel();
            result = merchPack.Status;
            Assert.Equal(MerchRequestStatus.Cancelled,result);
        }
        
        [Fact]
        public void Status_Invalid_ThrowsException()
        {
            MerchPack merch = new MerchPack(MerchType.ConferenceListener, ClothingSize.XS, employee);
            
            Action act = () => merchPack.StockAwaitDelivery();
            MerchStatusException exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockAwaitedDelivery.", exception.Message);
            
            act = () => merchPack.StockConfirm();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockConfirmed.", exception.Message);
            
            act = () => merchPack.StockReserve();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockReserved.", exception.Message);
            
            merchPack.Validate();
            
            act = () => merchPack.Validate();;
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to Validated.", exception.Message);
            
            act = () => merchPack.StockReserve();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockReserved.", exception.Message);

            merchPack.StockConfirm();
            
            act = () => merchPack.Validate();;
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to Validated.", exception.Message);
            
            act = () => merchPack.StockAwaitDelivery();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockAwaitedDelivery.", exception.Message);
            
            act = () => merchPack.StockConfirm();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockConfirmed.", exception.Message);
            
            merchPack.StockReserve();
            
            act = () => merchPack.Validate();;
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to Validated.", exception.Message);
            
            act = () => merchPack.StockAwaitDelivery();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockAwaitedDelivery.", exception.Message);
            
            act = () => merchPack.StockConfirm();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockConfirmed.", exception.Message);
            
            act = () => merchPack.StockReserve();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockReserved.", exception.Message);
            
            merchPack.Cancel();
            
            act = () => merchPack.Validate();;
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to Validated.", exception.Message);
            
            act = () => merchPack.StockAwaitDelivery();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockAwaitedDelivery.", exception.Message);
            
            act = () => merchPack.StockConfirm();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockConfirmed.", exception.Message);
            
            act = () => merchPack.StockReserve();
            exception = Assert.Throws<MerchStatusException>(act);
            Assert.Equal($"Incorrect request status. Status {merchPack.Status} cannot be changed to StockReserved.", exception.Message);
        }
    }
}