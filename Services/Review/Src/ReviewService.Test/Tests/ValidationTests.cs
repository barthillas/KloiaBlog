using System.Linq;
using ReviewService.Abstraction.Command;
using ReviewService.Abstraction.Validation;
using FluentAssertions;
using ReviewService.Domain.Entities;
using Xunit;

namespace ReviewService.UnitTest.Tests
{
    public class ValidationTests : ReviewTestBase
    {
        private CreateReviewCommandValidator CreateReviewCommandValidator { get; set; }
        private UpdateReviewCommandValidator UpdateReviewCommandValidator { get; }
        private DeleteReviewCommandValidator DeleteReviewCommandValidator { get; }

        private readonly Review _review;
        
        public ValidationTests()
        {
            CreateReviewCommandValidator = new CreateReviewCommandValidator();
            UpdateReviewCommandValidator = new UpdateReviewCommandValidator();
            DeleteReviewCommandValidator = new DeleteReviewCommandValidator();
            _review = FakeReviews.First();
        }
        [Fact]
        public void UpdateReviewCommandValidator_EmptyCommandObject_ShouldReturnTrue()
        {
            var request = new UpdateReviewCommand
            {
                ReviewContent = _review.ReviewContent,
                Reviewer = _review.Reviewer,
                ReviewId = _review.ReviewId
            };

            UpdateReviewCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateReviewCommandValidator_EmptyCommandObject_ShouldReturnTrue()
        {
            var request = new CreateReviewCommand  {
                ReviewContent = _review.ReviewContent,
                Reviewer = _review.Reviewer,
                ArticleId = _review.ArticleId
            };

            CreateReviewCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }


        [Fact]
        public void DeleteReviewCommandValidator_EmptyCommandObject_ShouldReturnTrue()
        {
            var request = new DeleteReviewCommand(){ReviewId = _review.ReviewId};

            DeleteReviewCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateReviewCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new UpdateReviewCommand();

            UpdateReviewCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }

        [Fact]
        public void UpdateReviewCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new CreateReviewCommand();

            CreateReviewCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }


        [Fact]
        public void DeleteReviewCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new DeleteReviewCommand();

            DeleteReviewCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }
    }
}