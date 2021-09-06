using System.Linq;
using Abstraction.Dto;
using ReviewService.Abstraction.Command;
using ReviewService.Abstraction.Validation;
using FluentAssertions;
using ReviewService.Domain.Entities;
using ReviewService.UnitTest.UnitTest;
using Xunit;

namespace ReviewService.UnitTest.Tests
{
    public class ValidationTests : ReviewTestBase
    {
        private CreateReviewCommandValidator _createReviewCommandValidator { get; set; }
        private UpdateReviewCommandValidator _updateReviewCommandValidator { get; }
        private DeleteReviewCommandValidator _deleteReviewCommandValidator { get; }

        private readonly Review _review;
        
        public ValidationTests()
        {
            _createReviewCommandValidator = new CreateReviewCommandValidator();
            _updateReviewCommandValidator = new UpdateReviewCommandValidator();
            _deleteReviewCommandValidator = new DeleteReviewCommandValidator();
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

            _updateReviewCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateReviewCommandValidator_EmptyCommandObject_ShouldReturnTrue()
        {
            var request = new CreateReviewCommand  {
                ReviewContent = _review.ReviewContent,
                Reviewer = _review.Reviewer,
                ArticleId = _review.ArticleId
            };

            _createReviewCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }


        [Fact]
        public void DeleteReviewCommandValidator_EmptyCommandObject_ShouldReturnTrue()
        {
            var request = new DeleteReviewCommand(){ReviewId = _review.ReviewId};

            _deleteReviewCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateReviewCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new UpdateReviewCommand();

            _updateReviewCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }

        [Fact]
        public void UpdateReviewCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new CreateReviewCommand();

            _createReviewCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }


        [Fact]
        public void DeleteReviewCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new DeleteReviewCommand();

            _deleteReviewCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }
    }
}