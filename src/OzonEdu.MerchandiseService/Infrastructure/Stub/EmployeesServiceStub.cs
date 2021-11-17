using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.Stub
{
    public static class EmployeesServiceStub
    {
        public static List<Employee> GetAll()
        {
            return new List<Employee>
            {
                new Employee(1, "Василий", "Васильевич","Васечкин", "vasya@mail.ru"),
                new Employee(2, "Иван", "Васильевич", "Иванов", "van@mail.ru"),
                new Employee(3, "Петр", "Васильевич", "Петров", "petr@mail.ru")
            };
        }
    }
}