using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Abstraction.Dto;
using ApiBase.Controllers;
using ApiBase.Response;
using ArticleService.Abstraction.Command;
using Data.CQRS;
using MediatR;

namespace ArticleService.Api.Controllers
{
    [ApiVersion("1.0")]
    public class ArticleController : ApiControllerBase
    {
        private readonly ICommandSender _commandSender;
        public ArticleController(ICommandSender mediator) 
        {
            _commandSender = mediator;
        }
        
        [HttpPost("Create")]
        public async Task<Response<ArticleDto>> Create([FromForm]CreateArticleCommand command)
        {
            var data = await _commandSender.SendAsync(command);
            return ProduceResponse(data);
        }
        
        [HttpDelete("Delete")]
        public async Task<Response<Unit>> Delete([FromForm]DeleteArticleCommand command)
        {
            var data = await _commandSender.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
        
        [HttpPut("Update")]
        public async Task<Response<Unit>> Update([FromForm]UpdateArticleCommand command)
        {
            var data = await _commandSender.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
    }
}
