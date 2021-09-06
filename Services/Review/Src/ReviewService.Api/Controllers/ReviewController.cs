using System.Threading.Tasks;
using Abstraction.Dto;
using ApiBase.Controllers;
using ApiBase.Response;
using Data.CQRS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Abstraction.Command;

namespace ReviewService.Api.Controllers
{
    [ApiVersion("1.0")]
    public class ReviewController : ApiControllerBase
    {
        private readonly ICommandSender _mediator;
        public ReviewController(ICommandSender mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("Create")]
        public async Task<Response<ReviewDto>> Create([FromForm] CreateReviewCommand command)
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
