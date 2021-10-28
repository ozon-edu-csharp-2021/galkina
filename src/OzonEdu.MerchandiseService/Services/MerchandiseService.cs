using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Models;
using OzonEdu.MerchandiseService.Services.Interfaces;

namespace OzonEdu.MerchandiseService.Services
{
    public class MerchandiseService : IMerchandiseService
    {
        public Task<bool> RequestMerchSet(int merchPackIndex, int size, CancellationToken token)
        {
            return Task.FromResult(true);
        }

        public Task<List<MerchSet>> RetrieveIssuedMerchSetsInformation(int employeeId, CancellationToken token)
        {
            List<MerchSet> merchSets = new List<MerchSet>
            {
                new MerchSet
                {
                MerchSetId = 1,
                MerchPack = "merch pack",
                Skues = new List<Sku> { new Sku { SkuId = 1, SkuName = "bag", Size = 1 } }
                }
            };

            return Task.FromResult(merchSets);
        }
    }
}