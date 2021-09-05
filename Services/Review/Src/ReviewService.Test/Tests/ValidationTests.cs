using ReviewService.Abstraction.Command;
using ReviewService.Abstraction.Validation;
using FluentAssertions;
using Xunit;

namespace ReviewService.UnitTest.Tests
{
    public class ValidationTests : TestBase.TestBase
    {
        private CreateReviewCommandValidator _createReviewCommandValidator { get; set; }
        private UpdateReviewCommandValidator _updateReviewCommandValidator { get; }
        private DeleteReviewCommandValidator _deleteReviewCommandValidator { get; }
        
        public ValidationTests()
        {
            _createReviewCommandValidator = new CreateReviewCommandValidator();
            _updateReviewCommandValidator = new UpdateReviewCommandValidator();
            _deleteReviewCommandValidator = new DeleteReviewCommandValidator();
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