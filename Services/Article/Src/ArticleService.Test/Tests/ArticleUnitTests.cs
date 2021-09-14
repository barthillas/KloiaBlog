using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abstraction.Dto;
using ApiBase.Response;
using ArticleService.Abstraction.Command;
using ArticleService.Api.Controllers;
using ArticleService.Domain.Entities;
using ArticleService.Infrastructure.Context;
using Data.CQRS;
using Data.Repositories.Data.Repositories;
using Data.UnitOfWork;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ArticleService.UnitTest.Tests
{
    public class ArticleTests : ArticleTestBase
    {
        private ArticleController _articleController;
        private readonly ArticleODataController _articleODataController;
        private readonly Mock<IRepository<Article>> _articleRepository;
        private readonly Mock<IUnitOfWork<ArticleDbContext>> _unitOfWork;
        private readonly Mock<ICommandSender> _mediator;

        public ArticleTests()
        {
            _mediator = new Mock<ICommandSender>();
            _articleRepository = new Mock<IRepository<Article>>();
            _articleRepository.Setup(x => x.Find(null)).Returns(FakeArticles);
            _unitOfWork =  new Mock<IUnitOfWork<ArticleDbContext>>();
            _unitOfWork.Setup(x => x.GetRepository<Article>()).Returns(_articleRepository.Object);
            _unitOfWork.Setup(x => x.GetRepository<Article>().CreateQuery()).Returns(FakeArticles.AsQueryable);
            _articleODataController = new ArticleODataController(_unitOfWork.Object);
        }

        [Fact]
        public void CheckRepository_Find_AllArticles()
        {
            var articles  = _articleRepository.Object.Find(null);

            articles.Should().BeSameAs(FakeArticles);

            Assert.IsAssignableFrom<IEnumerable<Article>>(articles);
        }
        
        [Fact]
        public void GetRepository_UnitOfWork_ShouldReturnRepository()
        {
            var articles = _unitOfWork.Object.GetRepository<Article>().Find(null);

            articles.Should().BeSameAs(FakeArticles);

            Assert.IsAssignableFrom<IEnumerable<Article>>(articles);
        }

        [Fact]
        public async void CreateArticle_ShouldReturn_CreatedArticle()
         {
             _mediator
                 .Setup( x=>  x.SendAsync( It.IsNotNull<CreateArticleCommand>(),default )) //ItExpr.IsAny<CancellationToken>())
                 .Returns(Task.FromResult(FakeArticleDtoList.First()));
             
            _articleController = new ArticleController(_mediator.Object);
            var article = FakeArticleDtoList.First();
            var response = await _articleController.Create(new CreateArticleCommand()
            {
                Author = article.Author,
                StarCount = 5,
                ArticleContent = article.ArticleContent,
                PublishDate = article.PublishDate.ToString(CultureInfo.InvariantCulture),
                Title = article.Title
            });

            response.Body.Should().Be(article);
            
            Assert.IsType<Response<ArticleDto>>(response);
        }
        
        [Fact]
        public async void UpdateArticle_ShouldReturn_ResponseUnit()
        {
            _mediator
                .Setup( x=>  x.SendAsync( It.IsNotNull<UpdateArticleCommand>(),default )) //ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(Unit.Value));
             
            _articleController = new ArticleController(_mediator.Object);
            var article = FakeArticleDtoList.First();
            var response = await _articleController.Update(new UpdateArticleCommand()
            {
                Author = article.Author ,
                StarCount = 5,
                ArticleContent = article.ArticleContent,
                PublishDate = article.PublishDate.ToString(CultureInfo.InvariantCulture),
                Title = article.Title
            });

            response.Body.Should().Be(Unit.Value);
            
            Assert.IsType<Response<Unit>>(response);
        }
        
        [Fact]
        public async void DeleteArticle_ShouldReturn_ResponseUnit()
        {
            _mediator
                .Setup( x=>  x.SendAsync( It.IsNotNull<DeleteArticleCommand>(),default )) //ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(Unit.Value));
             
            _articleController = new ArticleController(_mediator.Object);
            var response = await _articleController.Update(new UpdateArticleCommand()
            {
                ArticleId = 1
            });

            response.Body.Should().Be(Unit.Value);
            
            Assert.IsType<Response<Unit>>(response);
        }
        
        [Fact]
        public void OData_ExistingArticles_ShouldReturnOkObjectResult()
        {
            var response =  _articleODataController.Get();
            Assert.IsType<OkObjectResult>(response);
        }
        
        [Fact]
        public void GetById_UnExistingArticle_ShouldReturnOkObjectResult()
        {
            var response = _articleODataController.Get(1000);
            var okResult = response as OkObjectResult;
            Assert.IsType<OkObjectResult>(okResult);
        }
    }
}