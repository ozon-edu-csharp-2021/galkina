using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Models
{
    public class MerchPack
    {
        public int MerchPackIndex { get; set; }
        
        public string MerchPackName { get; set; }
        
        public List<Item> Items { get; set; }
    }
}