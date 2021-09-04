using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Dto;
using ApiBase.Controllers;
using ArticleService.Domain.Entities;
using ArticleService.Infrastructure.Context;
using Data.UnitOfWork;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace ArticleService.Api.Controllers
{
    [Route("[controller]")]
    public class ArticleController : ODataControllerBase
    {
        private readonly IUnitOfWork<ArticleDbContext> _unitOfWork;

        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger, ArticleDbContext context, IUnitOfWork<ArticleDbContext> unitOfWork) 
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        
        [EnableQuery]
        [ODataRoute("Articles({id})")]
        public async Task<IActionResult> Get([FromODataUri] int id)
        {
            var data = _unitOfWork.GetRepository<Article>()
                .Find(x => x.ArticleId == id)
                    .Select(x => new ArticleDto
                    {ArticleId = x.ArticleId, Author = x.Author, ArticleContent = x.ArticleContent});
            
            return Ok(data);
        }

       [ODataRoute("Articles")]
       [EnableQuery]
       public async Task<IActionResult> Get()
       {

           var asd = await _unitOfWork.GetRepository<Article>().GetAllAsync(new CancellationToken())
               .ConfigureAwait(false);
           
           
           return Ok( _unitOfWork.GetRepository<Article>().GetAll());
       }
    }
}
