using System;

namespace OzonEdu.MerchandiseService.Infrastructure.Models
{
    public class EmployeeDto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public DateTime birthDay { get; set; }
        public DateTime hiringDate { get; set; }
        public string email { get; set; }
        public int clothingSize { get; set; }
        public string[] conferences { get; set; }
        public long id { get; set; }
    }
}