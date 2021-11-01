using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Models
{
    public class MerchSet
    {
        public int MerchSetId { get; set; }

        public string MerchPack { get; set; }
        
        public List<Sku> Skues { get; set; }
    }
}