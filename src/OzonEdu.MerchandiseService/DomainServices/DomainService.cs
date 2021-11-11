using System;
using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.DomainServices
{
    public sealed class DomainService
    {
        public static bool DidMerchPackIssue(List<MerchPack> merchPacks, MerchType type)
        {
            MerchPack merchPack = merchPacks.Where(m => m.Type == type)
                .OrderByDescending(m => m.IssueDate)
                .FirstOrDefault();

            if (merchPack == null)
                return false;

            TimeSpan oneYear = new TimeSpan(365, 0, 0, 0);

            if (DateTime.Now - merchPack.IssueDate > oneYear)
                return false;

            return true;
        }

        public static Employee DoesEmployeeExist(List<Employee> employees, long employeeId)
        {
            Employee employee = employees.Where(e => e.Id == employeeId).FirstOrDefault();
            return employee;
        }
    }
}