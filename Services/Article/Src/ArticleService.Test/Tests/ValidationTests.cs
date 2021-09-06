using System.Linq;
using ArticleService.Abstraction.Command;
using ArticleService.Abstraction.Validation;
using ArticleService.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ArticleService.UnitTest.Tests
{
    public class ValidationTests : ArticleTestBase
    {
        private CreateArticleCommandValidator _createArticleCommandValidator { get; }
        private UpdateArticleCommandValidator _updateArticleCommandValidator { get; }
        private DeleteArticleCommandValidator _deleteArticleCommandValidator { get; }
        
        private readonly Article _article;
        public ValidationTests()
        {
            _createArticleCommandValidator = new CreateArticleCommandValidator();
            _updateArticleCommandValidator = new UpdateArticleCommandValidator();
            _deleteArticleCommandValidator = new DeleteArticleCommandValidator();
            _article = FakeArticles.First();
        }
        
        [Fact]
        public void UpdateArticleCommandValidator_EmptyCommandObject_ShouldReturnTrue()
        {
            var request = new UpdateArticleCommand
            {
                 ArticleId = _article.ArticleId,
                 Title = _article.Title,
                 ArticleContent = _article.ArticleContent,
                 Author = _article.Author,
                 PublishDate = "12",
                 StarCount = _article.StarCount,
            };

            _updateArticleCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateArticleCommandValidator_EmptyCommandObject_ShouldReturnTrue()
        {
            var request = new CreateArticleCommand  {
                Title = _article.Title,
                ArticleContent = _article.ArticleContent,
                Author = _article.Author,
                PublishDate = "12",
                StarCount = _article.StarCount,
            };

            _createArticleCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }


        [Fact]
        public void DeleteArticleCommandValidator_EmptyCommandObject_ShouldReturnTrue()
        {
            var request = new DeleteArticleCommand(){ArticleId = _article.ArticleId};

            _deleteArticleCommandValidator.Validate(request).IsValid.Should().BeTrue();
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