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
        private CreateArticleCommandValidator CreateArticleCommandValidator { get; }
        private UpdateArticleCommandValidator UpdateArticleCommandValidator { get; }
        private DeleteArticleCommandValidator DeleteArticleCommandValidator { get; }
        
        private readonly Article _article;
        public ValidationTests()
        {
            CreateArticleCommandValidator = new CreateArticleCommandValidator();
            UpdateArticleCommandValidator = new UpdateArticleCommandValidator();
            DeleteArticleCommandValidator = new DeleteArticleCommandValidator();
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

            UpdateArticleCommandValidator.Validate(request).IsValid.Should().BeTrue();
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

            CreateArticleCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }


        [Fact]
        public void DeleteArticleCommandValidator_EmptyCommandObject_ShouldReturnTrue()
        {
            var request = new DeleteArticleCommand(){ArticleId = _article.ArticleId};

            DeleteArticleCommandValidator.Validate(request).IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateArticleCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new UpdateArticleCommand();

            UpdateArticleCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }

        [Fact]
        public void UpdateArticleCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new CreateArticleCommand();

            CreateArticleCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }


        [Fact]
        public void DeleteArticleCommandValidator_EmptyCommandObject_ShouldReturnFalse()
        {
            var request = new DeleteArticleCommand();

            DeleteArticleCommandValidator.Validate(request).IsValid.Should().BeFalse();
        }
    }
}