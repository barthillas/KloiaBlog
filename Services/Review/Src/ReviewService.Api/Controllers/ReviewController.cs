
using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ODataClient _oDataClient;

        public ReviewController(IUnitOfWork<ReviewDbContext> unitOfWork, ODataClient oDataClient, ICommandSender mediator, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _oDataClient = oDataClient;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [EnableQuery]
        [ODataRoute("Reviews({id})")]
        public async Task<IActionResult> Get([FromODataUri] int id)
        {
            var reviewDtos = _unitOfWork.GetRepository<Review>().CreateQuery().Where(x => x.ReviewId == id).Select(x =>
                new ReviewDto
                {
                    ArticleId = x.ArticleId, Reviewer = x.Reviewer, ReviewId = x.ReviewId,
                    ReviewContent = x.ReviewContent
                }).ToList();

            if (!reviewDtos.Any())
            {
                return NotFound();
            }
            return Ok(MergeReviewWithArticle(reviewDtos, await GetArticle().ConfigureAwait(false)));
        }

        [ODataRoute("Reviews")]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var isInbound = _httpContextAccessor.HttpContext?.Request.Headers["InboundRequest"].Count;
            if (isInbound > 0)
            {
                return Ok( _unitOfWork.GetRepository<Review>().CreateQuery().ToList());
            }
            var reviews = await _unitOfWork.GetRepository<Review>().CreateQuery().Select(x => new ReviewDto
                    {ArticleId = x.ArticleId, ReviewId = x.ReviewId, Reviewer = x.Reviewer, ReviewContent = x.ReviewContent}).ToListAsync( new CancellationToken())
                .ConfigureAwait(false);
            return Ok(MergeReviewWithArticle(reviews, await GetArticle().ConfigureAwait(false)));
        }
        
        [ApiExplorerSettings(IgnoreApi=false)] 
        [HttpPost("Create")]
        public async Task<Response<Unit>> Create([FromForm] CreateReviewCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
        
        [ApiExplorerSettings(IgnoreApi=false)] 
        [HttpDelete("Delete")]
        public async Task<Response<Unit>> Delete([FromForm] DeleteReviewCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
        
        [ApiExplorerSettings(IgnoreApi=false)] 
        [HttpPut("Update")]
        public async Task<Response<Unit>> Update([FromForm] UpdateReviewCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }
        
        private async Task<IEnumerable<IDictionary<string, object>>> GetArticle()
        {
            var isInbound = _httpContextAccessor.HttpContext?.Request.Headers["InboundRequest"].Count;
            if (isInbound > 0)
            {
                return new List<IDictionary<string, object>>();
            }
            return await _oDataClient
                .For("Article")
                .FindEntriesAsync(new ODataFeedAnnotations()).ConfigureAwait(false);
        }

        private static IQueryable<ReviewDto> MergeReviewWithArticle(IEnumerable<ReviewDto> data, IEnumerable<IDictionary<string, object>> articles)
        {
            var reviewDtos = data.ToList();
            foreach (var reviewDto in reviewDtos)
            {
                var enumerable = articles.Where(x => (int) x["ArticleId"] == reviewDto.ArticleId).ToList();
                if (!enumerable.Any()) continue;
                var article = enumerable.Select(dictionary => new ArticleDto()
                    {
                        ArticleId = (int) dictionary["ArticleId"],
                        Author = (string) dictionary["Author"],
                        ArticleContent = (string) dictionary["ArticleContent"],
                        Title = (string) dictionary["Title"],
                    })
                    .FirstOrDefault();

                reviewDto.Article = article;
            }

            return reviewDtos.AsQueryable();
        } 
    }
}
