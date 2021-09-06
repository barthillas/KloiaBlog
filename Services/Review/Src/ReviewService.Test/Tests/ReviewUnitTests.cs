using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abstraction.Dto;
using ApiBase.Response;
using Data.CQRS;
using Data.Repositories.Data.Repositories;
using Data.UnitOfWork;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReviewService.Abstraction.Command;
using ReviewService.Api.Controllers;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;
using ReviewService.UnitTest.UnitTest;
using Xunit;

namespace ReviewService.UnitTest.Tests
{
    public class ReviewTests : ReviewTestBase
    {
        private ReviewController _reviewController;
        private readonly ReviewODataController _reviewODataController;
        private readonly Mock<IRepository<Review>> _reviewRepository;
        private readonly Mock<IUnitOfWork<ReviewDbContext>> _unitOfWork;
        private readonly Mock<ICommandSender> _mediator;

        public ReviewTests()
        {
            _mediator = new Mock<ICommandSender>();
            _reviewRepository = new Mock<IRepository<Review>>();
            _reviewRepository.Setup(x => x.Find(null)).Returns(FakeReviews);
            _unitOfWork =  new Mock<IUnitOfWork<ReviewDbContext>>();
            _unitOfWork.Setup(x => x.GetRepository<Review>()).Returns(_reviewRepository.Object);
            _unitOfWork.Setup(x => x.GetRepository<Review>().CreateQuery()).Returns(FakeReviews.AsQueryable);
            _reviewODataController = new ReviewODataController(_unitOfWork.Object);
        }

        [Fact]
        public void CheckRepository_Find_AllReviews()
        {
            var reviews  = _reviewRepository.Object.Find(null);

            reviews.Should().BeSameAs(FakeReviews);

            Assert.IsAssignableFrom<IEnumerable<Review>>(reviews);
        }
        
        [Fact]
        public void GetRepository_UnitOfWork_ShouldReturnRepository()
        {
            var reviews = _unitOfWork.Object.GetRepository<Review>().Find(null);

            reviews.Should().BeSameAs(FakeReviews);

            Assert.IsAssignableFrom<IEnumerable<Review>>(reviews);
        }

        [Fact]
        public async void CreateReview_ShouldReturn_CreatedReview()
         {
             _mediator
                 .Setup( x=>  x.SendAsync( It.IsNotNull<CreateReviewCommand>(),default )) //ItExpr.IsAny<CancellationToken>())
                 .Returns(Task.FromResult(FakeReviewDtoList.First()));
             
            _reviewController = new ReviewController(_mediator.Object);
            var review = FakeReviewDtoList.First();
            var response = await _reviewController.Create(new CreateReviewCommand()
            {
                Reviewer = review.Reviewer ,
                ArticleId = review.ArticleId,
                ReviewContent = review.ReviewContent,
            });

            response.Body.Should().Be(review);
            
            Assert.IsType<Response<ReviewDto>>(response);
        }
        
        [Fact]
        public async void UpdateReview_ShouldReturn_ResponseUnit()
        {
            _mediator
                .Setup( x=>  x.SendAsync( It.IsNotNull<UpdateReviewCommand>(),default )) //ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(Unit.Value));
             
            _reviewController = new ReviewController(_mediator.Object);
            var review = FakeReviewDtoList.First();
            var response = await _reviewController.Update(new UpdateReviewCommand()
            {
                Reviewer = review.Reviewer ,
                ReviewId = review.ReviewId,
                ReviewContent = review.ReviewContent,
            });

            response.Body.Should().Be(Unit.Value);
            
            Assert.IsType<Response<Unit>>(response);
        }
        
        [Fact]
        public async void DeleteReview_ShouldReturn_ResponseUnit()
        {
            _mediator
                .Setup( x=>  x.SendAsync( It.IsNotNull<DeleteReviewCommand>(),default )) //ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(Unit.Value));
             
            _reviewController = new ReviewController(_mediator.Object);
            var review = FakeReviewDtoList.First();
            var response = await _reviewController.Update(new UpdateReviewCommand()
            {
                ReviewId = 1
            });

            response.Body.Should().Be(Unit.Value);
            
            Assert.IsType<Response<Unit>>(response);
        }
        
        [Fact]
        public void OData_ExistingReviews_ShouldReturnOkObjectResult()
        {
            var response =  _reviewODataController.Get();
            Assert.IsType<OkObjectResult>(response);
        }
        
        [Fact]
        public void GetById_UnExistingReview_ShouldReturnOkObjectResult()
        {
            var response = _reviewODataController.Get(1000);
            var okResult = response as OkObjectResult;
            Assert.IsType<OkObjectResult>(okResult);
        }
    }
}