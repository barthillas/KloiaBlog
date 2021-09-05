using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Simple.OData.Client;

namespace ArticleService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ArticleController : ODataControllerBase
    {
        private readonly IUnitOfWork<ArticleDbContext> _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ArticleController> _logger;
        private readonly ICommandSender _mediator;

        public ArticleController(ILogger<ArticleController> logger, ArticleDbContext context, IUnitOfWork<ArticleDbContext> unitOfWork, IConfiguration configuration, ICommandSender mediator) 
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mediator = mediator;
        }

        
        [EnableQuery]
        [ODataRoute("Articles({id})")]
        public async Task<IActionResult> Get([FromODataUri] int id)
        {
            var articles = await _unitOfWork.GetRepository<Article>().CreateQuery(x => x.ArticleId == id).Select(x => new ArticleDto
                    {ArticleId = x.ArticleId, Author = x.Author, ArticleContent = x.ArticleContent}).ToListAsync( new CancellationToken())
                .ConfigureAwait(false);
            return Ok(MergeArticleWithReview(articles, await GetReviews().ConfigureAwait(false)));
        }

        [ODataRoute("Articles")]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var articles = await _unitOfWork.GetRepository<Article>().CreateQuery().Select(x => new ArticleDto
                    {ArticleId = x.ArticleId, Author = x.Author, ArticleContent = x.ArticleContent}).ToListAsync( new CancellationToken())
                .ConfigureAwait(false);
            
            return Ok(MergeArticleWithReview(articles, await GetReviews().ConfigureAwait(false)));
        }
        
        [HttpPost("Create")]
        public async Task<Response<Unit>> Create([FromForm] CreateArticleCommand command)
        {
            var data = await _mediator.SendAsync(command).ConfigureAwait(false);
            return ProduceResponse(data);
        }

        private async Task<IEnumerable<IDictionary<string, object>>> GetReviews()
        {
            var client = new ODataClient(new ODataClientSettings(new Uri(_configuration["ReviewOdataHost:Url"]))
            {
                IgnoreResourceNotFoundException = true,
                OnTrace = (x, y) => Console.WriteLine(string.Format(x, y)),
            });
            return await client
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
