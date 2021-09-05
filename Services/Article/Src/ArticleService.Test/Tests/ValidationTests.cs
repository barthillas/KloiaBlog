using ArticleService.Abstraction.Command;
using ArticleService.Abstraction.Validation;
using FluentAssertions;
using Xunit;

namespace ArticleService.UnitTest.Controllers.Tests
{
    public class ValidationTests : TestBase.TestBase
    {
        private CreateArticleCommandValidator _createArticleCommandValidator { get; set; }
        private UpdateArticleCommandValidator _updateArticleCommandValidator { get; }
        private DeleteArticleCommandValidator _deleteArticleCommandValidator { get; }
        
        public ValidationTests()
        {
            _createArticleCommandValidator = new CreateArticleCommandValidator();
            _updateArticleCommandValidator = new UpdateArticleCommandValidator();
            _deleteArticleCommandValidator = new DeleteArticleCommandValidator();
        }

        [Fact]
        public void CreateArticleCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new UpdateArticleCommand();

            _updateArticleCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }

        [Fact]
        public void UpdateArticleCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new CreateArticleCommand();

            _createArticleCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }


        [Fact]
        public void DeleteArticleCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new DeleteArticleCommand();

            _deleteArticleCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }
    }
}