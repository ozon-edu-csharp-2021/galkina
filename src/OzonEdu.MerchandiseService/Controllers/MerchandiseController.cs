using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.HttpModels;
using OzonEdu.MerchandiseService.Services.Interfaces;

namespace OzonEdu.MerchandiseService.Controllers
{
    [ApiController]
    [Route("api/merchandise")]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMerchandiseService _merchandiseService;

        public MerchandiseController(IMerchandiseService merchandiseService)
        {
            _merchandiseService = merchandiseService;
        }
        
        /// <summary>
        /// Requests the merch set for employee.
        /// </summary>
        /// <param name="merchPackIndex">Index of the merch pack variant.</param>
        /// <param name="size">Clothing size.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{merchPackIndex:int}/{size:int}")]
        public async Task<ActionResult<MerchSetResponse>> QueryMerchSet(int merchPackIndex, int size, CancellationToken token)
        {
            var merchSet = await _merchandiseService.QueryMerchSet(merchPackIndex, size, token);
            MerchSetResponse merchSetResponse = new MerchSetResponse
            {
                MerchSetId = merchSet.MerchSetId,
                MerchPack = merchSet.MerchPack,
                Skues = merchSet.Skues.Select(i => new HttpModels.Sku
                {
                    SkuId = i.SkuId,
                    SkuName = i.SkuName,
                    Size = i.Size
                }).ToList()
            };
                    
            return Ok(merchSetResponse);
        }

        /// <summary>
        /// Returns information about which sets of merch were issued to the employee.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{employeeId:int}")]
        public async Task<List<MerchSetResponse>> RetrieveIssuedMerchSetsInformation(int employeeId, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}