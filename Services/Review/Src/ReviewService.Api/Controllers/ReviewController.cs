using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Dto;
using ApiBase.Controllers;
using Data.UnitOfWork;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;

namespace ReviewService.Api.Controllers
{
    [Route("[controller]")]
    public class ReviewController : ODataControllerBase
    {
        private readonly IUnitOfWork<ReviewDbContext> _unitOfWork;

        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ILogger<ReviewController> logger, IUnitOfWork<ReviewDbContext> unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
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

            var asd = await _unitOfWork.GetRepository<Review>().GetAllAsync(new CancellationToken())
                .ConfigureAwait(false);
           
           
            return Ok( _unitOfWork.GetRepository<Review>().GetAll());
        }
    }
}
