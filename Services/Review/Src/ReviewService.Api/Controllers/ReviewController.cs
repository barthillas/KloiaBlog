
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Dto;
using ApiBase.Controllers;
using ApiBase.Response;
using ArticleService.Abstraction.Command;
using Data.CQRS;
using Data.UnitOfWork;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Abstraction.Command;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;
using Simple.OData.Client;

namespace ReviewService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ReviewController : ODataControllerBase
    {
        private readonly IUnitOfWork<ReviewDbContext> _unitOfWork;
        private readonly ICommandSender _mediator;
        private readonly ODataClient _oDataClient;

        public ReviewController(IUnitOfWork<ReviewDbContext> unitOfWork, ODataClient oDataClient, ICommandSender mediator)
        {
            _unitOfWork = unitOfWork;
            _oDataClient = oDataClient;
            _mediator = mediator;
        }

        [EnableQuery]
        [ODataRoute("Reviews({id})")]
        public async Task<IActionResult> Get([FromODataUri] int id)
        {
            var data = _unitOfWork.GetRepository<Review>()
                .Find(x => x.ReviewId == id)
                .Select(x => new ReviewDto()
                    {ArticleId = x.ArticleId, Reviewer = x.Reviewer, ReviewId = x.ReviewId, ReviewContent = x.ReviewContent});
            
            return Ok(data);
        }

        [ODataRoute("Reviews")]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {

            var data = await _unitOfWork.GetRepository<Review>().GetAllAsync(new CancellationToken())
                .ConfigureAwait(false);
           
           
            return Ok( _unitOfWork.GetRepository<Review>().GetAll());
        }
        
        [HttpPost("Create")]
        public async Task<Response<Unit>> Create([FromForm] CreateReviewCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
        
        [HttpDelete("Delete")]
        public async Task<Response<Unit>> Delete([FromForm] DeleteReviewCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
        
        [HttpPut("Update")]
        public async Task<Response<Unit>> Update([FromForm] UpdateReviewCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
    }
}
