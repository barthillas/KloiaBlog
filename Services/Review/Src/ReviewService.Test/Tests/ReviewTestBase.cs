using System.Collections.Generic;
using Abstraction.Dto;
using ReviewService.Domain.Entities;

namespace ReviewService.UnitTest.Tests
{
    public class ReviewTestBase : TestBase.TestBase
    {
        public List<Review> FakeReviews = new()
        {
            new Review{
                ReviewId = 1,
                ArticleId = 1, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new Review{
                ReviewId = 2,
                ArticleId = 2, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new Review{
                ReviewId = 3,
                ArticleId = 1, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new Review{
                ReviewId = 4,
                ArticleId = 4, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new Review{
                ReviewId = 5,
                ArticleId = 5, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new Review{
                ReviewId = 6,
                ArticleId = 2, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
        };

        public List<ReviewDto> FakeReviewDtoList = new()
        {
            new ReviewDto{
                ReviewId = 1,
                ArticleId = 1, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new ReviewDto{
                ReviewId = 2,
                ArticleId = 2, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new ReviewDto{
                ReviewId = 3,
                ArticleId = 1, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new ReviewDto{
                ReviewId = 4,
                ArticleId = 4, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new ReviewDto{
                ReviewId = 5,
                ArticleId = 5, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
            new ReviewDto{
                ReviewId = 6,
                ArticleId = 2, 
                Reviewer = "John Doe",
                ReviewContent = "OMG perfect!!",
            },
        };
    }
}