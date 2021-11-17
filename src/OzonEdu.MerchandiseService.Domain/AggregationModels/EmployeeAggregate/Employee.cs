using System;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public class Employee : Entity
    {
        public Name FirstName { get; set; }
        public Name LastName { get; set; }
        public Name MiddleName { get; set; }
        public Email Email { get; set; }

        public Employee(long id, string firstName, string middleName, string lastName, string email)
        {
            Id = id;
            FirstName = Name.Create(firstName);
            LastName = Name.Create(lastName);
            MiddleName = Name.Create(middleName);
            Email = Email.Create(email);
        }
        
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
        }
    }
}