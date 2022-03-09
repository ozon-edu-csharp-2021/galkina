using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.HttpModels
{
    public class MerchPackResponse
    {
        public MerchPackType Type { get; set; }
        public ClothingSize ClothingSize { get; set; }
        public MerchRequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? IssueDate { get; set; }
    }
}