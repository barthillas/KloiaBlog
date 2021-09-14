using System.Linq;
using ArticleService.Domain.Entities;
using ArticleService.Infrastructure.Context;
using Data.UnitOfWork;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Api.Controllers
{
    public class ArticleODataController : ODataController
    {
        private readonly IUnitOfWork<ArticleDbContext> _unitOfWork;

        public ArticleODataController(IUnitOfWork<ArticleDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [EnableQuery]
        [ODataRoute("Articles")]
        public IActionResult Get()
        {
            var albums = _unitOfWork.GetRepository<Article>().CreateQuery();
            return Ok(albums);
        }

        [EnableQuery]
        [ODataRoute("Articles({id})")]
        public IActionResult Get(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var album = _unitOfWork.GetRepository<Article>().CreateQuery().Where(x => x.ArticleId == id);
            
            return Ok(album);
        }
    }
}