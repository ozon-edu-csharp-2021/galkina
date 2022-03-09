using System;

namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Models
{
    public class MerchPack
    {
        public long Id { get; set; }
        public int MerchTypeId { get; set; }
        public int? ClothingSize { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? IssueDate { get; set; }
    }
}