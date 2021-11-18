using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.HttpModels
{
    public class MerchPack
    {
        public int MerchPackIndex { get; set; }
        
        public string MerchPackName { get; set; }
        
        public List<Item> Items { get; set; }
    }
}