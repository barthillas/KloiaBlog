using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Dto;
using ApiBase.Controllers;
using ApiBase.Response;
using ArticleService.Abstraction.Command;
using ArticleService.Domain.Entities;
using ArticleService.Infrastructure.Context;
using Data.CQRS;
using Data.UnitOfWork;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Simple.OData.Client;

namespace ArticleService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ArticleController : ODataControllerBase
    {
        private readonly IUnitOfWork<ArticleDbContext> _unitOfWork;
        private readonly ICommandSender _mediator;
        private readonly ODataClient _oDataClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public ArticleController( IUnitOfWork<ArticleDbContext> unitOfWork, ICommandSender mediator, ODataClient oDataClient, IHttpContextAccessor httpContextAccessor) 
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _oDataClient = oDataClient;
            _httpContextAccessor = httpContextAccessor;
        }

        
        [EnableQuery]
        [ODataRoute("Articles({id})")]
        public async Task<IActionResult> Get([FromODataUri] int id)
        {
            var articles = _unitOfWork.GetRepository<Article>().CreateQuery().Where(x => x.ArticleId == id).Select(x => new ArticleDto
                    {ArticleId = x.ArticleId, Author = x.Author, ArticleContent = x.ArticleContent}).ToList();
            var data = MergeArticleWithReview(articles, await GetReviews().ConfigureAwait(false));
            if (!data.Any())
            {
                return NotFound();
            }

            return Ok(data);
        }

        [ODataRoute("Articles")]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var isInbound = _httpContextAccessor.HttpContext?.Request.Headers["InboundRequest"].Count;
            if (isInbound > 0)
            {
                return Ok( _unitOfWork.GetRepository<Article>().CreateQuery().ToList());
            }

            var articles = await _unitOfWork.GetRepository<Article>().CreateQuery().Select(x => new ArticleDto
                    {ArticleId = x.ArticleId, Author = x.Author, ArticleContent = x.ArticleContent}).ToListAsync()
                .ConfigureAwait(false);
            
            return Ok(MergeArticleWithReview(articles, await GetReviews().ConfigureAwait(false)));
        }
        
        [HttpPost("Create")]
        public async Task<Response<Unit>> Create([FromForm] CreateArticleCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
        
        [HttpDelete("Delete")]
        public async Task<Response<Unit>> Delete([FromForm] DeleteArticleCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
        
        [HttpPut("Update")]
        public async Task<Response<Unit>> Update([FromForm] UpdateArticleCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }

        private async Task<IEnumerable<IDictionary<string, object>>> GetReviews()
        {
            var isInbound = _httpContextAccessor.HttpContext?.Request.Headers["InboundRequest"].Count;
            if (isInbound > 0)
            {
                return new List<IDictionary<string, object>>();
            }
            return await _oDataClient
                .For("Review")
                .FindEntriesAsync(new ODataFeedAnnotations()).ConfigureAwait(false);
        }

        private static IQueryable<ArticleDto> MergeArticleWithReview(IEnumerable<ArticleDto> data, IEnumerable<IDictionary<string, object>> reviews)
        {
            var articleDtos = data.ToList();
            foreach (var articleDto in articleDtos)
            {
                var enumerable = reviews.Where(x => (int) x["ArticleId"] == articleDto.ArticleId).ToList();
                if (!enumerable.Any()) continue;
                var reviewDtos = enumerable.Select(dictionary => new ReviewDto()
                    {
                        ArticleId = (int) dictionary["ArticleId"],
                        Article = articleDto,
                        ReviewId = (int) dictionary["ReviewId"],
                        ReviewContent = (string) dictionary["ReviewContent"],
                        Reviewer = (string) dictionary["Reviewer"],
                    })
                    .ToList();

                articleDto.Reviews = reviewDtos;
            }

            return articleDtos.AsQueryable();
        } 

    }
}
