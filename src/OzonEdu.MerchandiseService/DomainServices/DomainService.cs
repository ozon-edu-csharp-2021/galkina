using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;

namespace OzonEdu.MerchandiseService.DomainServices
{
    public sealed class DomainService
    {
        public static bool TryGetEmployee(List<Employee> employees, long employeeId, out Employee employee)
        {
            employee = employees.Where(e => e.Id == employeeId).FirstOrDefault();

            return employee is not null;
        }
    }
}