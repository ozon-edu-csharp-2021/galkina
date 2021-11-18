using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Domain.Exceptions;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPack : Entity, IAggregationRoot
    {
        public MerchType Type { get; }
        public ClothingSize ClothingSize { get; }
        public SkuList Items { get; }
        public Employee Employee { get; }
        public MerchRequestStatus Status { get; private set; }
        public DateTime RequestDate { get; }
        public DateTime? IssueDate { get; private set; }
        
        public MerchPack(MerchType type, ClothingSize size, Employee employee)
        {
            Type = type;
            ClothingSize = size;
            Items = new SkuList(Type, ClothingSize);
            Employee = employee;
            Status = MerchRequestStatus.Submitted;
            RequestDate = DateTime.Now;
        }
        
        public MerchPack(long id, MerchType type, ClothingSize size, Employee employee, DateTime requestDate)
        {
            Id = id;
            Type = type;
            ClothingSize = size;
            Items = new SkuList(Type, ClothingSize);
            Employee = employee;
            Status = MerchRequestStatus.StockAwaitedDelivery;
            RequestDate = requestDate;
        }
        
        public MerchPack(long id, MerchType type, ClothingSize size, Employee employee, DateTime requestDate, DateTime? issueDate)
        {
            Id = id;
            Type = type;
            ClothingSize = size;
            Items = new SkuList(Type, ClothingSize);
            Employee = employee;
            Status = MerchRequestStatus.StockReserved;
            RequestDate = requestDate;
            IssueDate = issueDate;
        }
        
        public void Validate()
        {
            if (Status != MerchRequestStatus.Submitted && Status != MerchRequestStatus.StockAwaitedDelivery)
            {
                throw new MerchStatusException($"Incorrect request status. Status {Status} cannot be changed to Validated.");
            }

            Status = MerchRequestStatus.Validated;
        }
        
        public void StockAwaitDelivery()
        {
            if (Status != MerchRequestStatus.Validated)
            {
                throw new MerchStatusException($"Incorrect request status. Status {Status} cannot be changed to StockAwaitedDelivery.");
            }

            Status = MerchRequestStatus.StockAwaitedDelivery;
        }
        
        public void StockConfirm()
        {
            if (Status != MerchRequestStatus.Validated)
            {
                throw new MerchStatusException($"Incorrect request status. Status {Status} cannot be changed to StockConfirmed.");
            }

            Status = MerchRequestStatus.StockConfirmed;
        }
        
        public void StockReserve()
        {
            if (Status != MerchRequestStatus.StockConfirmed)
            {
                throw new MerchStatusException($"Incorrect request status. Status {Status} cannot be changed to StockReserved.");
            }

            Status = MerchRequestStatus.StockReserved;
            IssueDate = DateTime.Now;
            AddMerchPackReservedDomainEvent();
        }
        
        public void Cancel()
        {
            Status = MerchRequestStatus.Cancelled;
        }

        private void AddMerchPackReservedDomainEvent()
        {
            var merchReservedDomainEvent = new MerchPackReservedDomainEvent(Employee.Email.ToString(), Employee.ToString(), Type);
            AddDomainEvent(merchReservedDomainEvent);
        }
    }
}