using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.HttpModels;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Queries;
using Aggregate = OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.Controllers
{
    [ApiController]
    [Route("api/merchandise")]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchandiseController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        /// <summary>
        /// Requests the merch set for employee.
        /// </summary>
        /// <param name="merchPackIndex">Index of the merch pack variant.</param>
        /// <param name="size">Clothing size.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{employeeId:long}/{merchPackIndex:int}/{size}")]
        public async Task<ActionResult<MerchPackResponse>> QueryMerchSet(long employeeId, int merchPackIndex, string size, CancellationToken token)
        {
            var giveMerchPackAtEmployeeRequestCommand = new GiveMerchPackAtEmployeeRequestCommand()
            {
                Type = Aggregate.MerchType.Parse(merchPackIndex),
                ClothingSize = Aggregate.ClothingSize.Parse(size),
                EmployeeId = employeeId
            };

            Aggregate.MerchPack result = await _mediator.Send(giveMerchPackAtEmployeeRequestCommand, token);
            
            MerchPackResponse merchPackResponse = new MerchPackResponse()
            {
                Type = (MerchPackType) result.Type.ParseToInt(),
                ClothingSize = (ClothingSize) result.ClothingSize.ParseToInt(),
                Status = (MerchRequestStatus) result.Status.ParseToInt(),
                RequestDate = result.RequestDate,
                IssueDate = result.IssueDate
            };
            
            return Ok(merchPackResponse);
        }

        /// <summary>
        /// Returns information about which sets of merch were issued to the employee.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{employeeId:long}")]
        public async Task<List<MerchPackResponse>> RetrieveIssuedMerchSetsInformation(long employeeId, CancellationToken token)
        {
            var getIssuedMerchPacksQuery = new GetIssuedMerchPacksQuery()
            {
                EmployeeId = employeeId
            };

            var result = await _mediator.Send(getIssuedMerchPacksQuery, token);

            List<MerchPackResponse> response = new List<MerchPackResponse>();

            foreach (Aggregate.MerchPack merchPack in result)
            {
                MerchPackResponse merchPackResponse = new MerchPackResponse()
                {
                    Type = (MerchPackType) merchPack.Type.ParseToInt(),
                    ClothingSize = (ClothingSize) merchPack.ClothingSize.ParseToInt(),
                    Status = (MerchRequestStatus) merchPack.Status.ParseToInt(),
                    RequestDate = merchPack.RequestDate,
                    IssueDate = merchPack.IssueDate
                };
                response.Add(merchPackResponse);
            }

            return response;
        }
    }
}