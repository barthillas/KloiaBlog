using System.Collections.Generic;
using System.Linq;
using ApiBase.Response;
using Data.CQRS;
using Data.Repositories.Data.Repositories;
using Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReviewService.Abstraction.Command;
using ReviewService.Api.Controllers;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;
using Simple.OData.Client;
using Xunit;

namespace ReviewService.UnitTest.Tests
{
    public class ReviewTests : TestBase.TestBase
    {
        private readonly ReviewController _articleController;
        private readonly Mock<IRepository<Review>> _articleRepository;
        private readonly Mock<IUnitOfWork<ReviewDbContext>> _unitOfWork;
        private readonly Mock<ICommandSender> _mediator;
        private readonly Mock<ODataClient> _oDataClient;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

        public ReviewTests()
        {
            var fakeReviews = new List<Review>
            {
                new Review {ReviewId = 1, Reviewer = "Reviewer", ArticleId = 1, ReviewContent = "ReviewContent"},
                new Review {ReviewId = 2, Reviewer = "Reviewer", ArticleId = 1, ReviewContent = "ReviewContent"},
                new Review {ReviewId = 3, Reviewer = "Reviewer", ArticleId = 2, ReviewContent = "ReviewContent"},
                new Review {ReviewId = 4, Reviewer = "Reviewer", ArticleId = 1, ReviewContent = "ReviewContent"},
                new Review {ReviewId = 5, Reviewer = "Reviewer", ArticleId = 2, ReviewContent = "ReviewContent"},
                new Review {ReviewId = 6, Reviewer = "Reviewer", ArticleId = 1, ReviewContent = "ReviewContent"},
            };
            
            _articleRepository = new Mock<IRepository<Review>>();
            _unitOfWork =  new Mock<IUnitOfWork<ReviewDbContext>>();
            _unitOfWork.Setup(x => x.GetRepository<Review>()).Returns(_articleRepository.Object);
            _unitOfWork.Setup(x => x.GetRepository<Review>().CreateQuery()).Returns( fakeReviews.AsQueryable);
            
            
            _mediator = new Mock<ICommandSender>();
            _oDataClient = new Mock<ODataClient>();
            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("InboundRequest", "InboundRequest");

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor 
                .SetupGet(accessor => accessor.HttpContext)
                .Returns(httpContext);
            _articleController = new ReviewController(_unitOfWork.Object, null,_mediator.Object, _httpContextAccessor.Object);
        }

        [Fact]
        public async void Get_ExistingReviews_ShouldReturnOkObjectResult()
        {
            var response = await _articleController.Get();

            Assert.IsType<OkObjectResult>(response);
        }
        
        [Fact]
        public async void Get_UnExistingReview_ShouldReturnNotFoundResult()
        {
            var response = await _articleController.Get(1000);
            var okResult = response as NotFoundResult;
            Assert.IsType<NotFoundResult>(okResult);
        }
        
        [Fact]
        public async void Update_Review_ShouldReturnUnit()
        {
            var response = await _articleController.Update(new UpdateReviewCommand());
            var result = response as Response<Unit>;
            Assert.IsType<Unit>(result.Body);
        }
        
        [Fact]
        public async void Update_UnExistingReview_ShouldReturnUnit()
        {
            var response = await _articleController.Update(new UpdateReviewCommand{ReviewId = 12, Reviewer = "Reviewer",ReviewContent = "ReviewContent"});
            var result = response as Response<Unit>;
            Assert.IsType<Unit>(result.Body);
        }

    }
}