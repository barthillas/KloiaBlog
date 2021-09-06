using System.Linq;
using ApiBase.Controllers;
using Data.UnitOfWork;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;

namespace ReviewService.Api.Controllers
{
    public class ReviewODataController : ODataController
    {
        private readonly IUnitOfWork<ReviewDbContext> _unitOfWork;

        public ReviewODataController(IUnitOfWork<ReviewDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [EnableQuery]
        [ODataRoute("Reviews")]
        public IActionResult Get()
        {
            var albums = _unitOfWork.GetRepository<Review>().CreateQuery();
            return Ok(albums);
        }

        [EnableQuery]
        [ODataRoute("Reviews({id})")]
        public IActionResult Get(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var album = _unitOfWork.GetRepository<Review>().CreateQuery().Where(x => x.ReviewId == id);
            return Ok(album);
        }
    }
}