﻿using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.HttpModels
{
    public class MerchSetResponse
    {
        public int MerchSetId { get; set; }

        public string MerchPack { get; set; }
        
        public List<Sku> Skues { get; set; }
    }
}