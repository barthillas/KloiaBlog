using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
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
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Simple.OData.Client;
using Xunit;

namespace ArticleService.UnitTest.Controllers
{
    public class ArticleTests : TestBase.TestBase
    {
        private readonly ArticleController _articleController;
        private readonly Mock<IRepository<Article>> _articleRepository;
        private readonly Mock<IUnitOfWork<ArticleDbContext>> _unitOfWork;
        private readonly Mock<ICommandSender> _mediator;
        private readonly Mock<ODataClient> _oDataClient;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

        public ArticleTests()
        {
            var fakeArticles = new List<Article>
            {
                new Article {ArticleId = 1, Title = "article"},
                new Article {ArticleId = 2, Title = "article"},
                new Article {ArticleId = 3, Title = "article"},
            };
            
            _articleRepository = new Mock<IRepository<Article>>();
            _unitOfWork =  new Mock<IUnitOfWork<ArticleDbContext>>();
            _unitOfWork.Setup(x => x.GetRepository<Article>()).Returns(_articleRepository.Object);
            _unitOfWork.Setup(x => x.GetRepository<Article>().CreateQuery()).Returns( fakeArticles.AsQueryable);
            
            
            _mediator = new Mock<ICommandSender>();
            _oDataClient = new Mock<ODataClient>();
            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("InboundRequest", "InboundRequest");

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor 
                .SetupGet(accessor => accessor.HttpContext)
                .Returns(httpContext);
            _articleController = new ArticleController(_unitOfWork.Object, _mediator.Object,null, _httpContextAccessor.Object);
        }

        [Fact]
        public async void Get_ExistingArticles_ShouldReturnOkObjectResult()
        {
            var response = await _articleController.Get();

            Assert.IsType<OkObjectResult>(response);
        }
        
        [Fact]
        public async void Get_UnExistingArticle_ShouldReturnNotFoundResult()
        {
            var response = await _articleController.Get(1000);
            var okResult = response as NotFoundResult;
            Assert.IsType<NotFoundResult>(okResult);
        }
        
        [Fact]
        public async void Update_Article_ShouldReturnUnit()
        {
            var response = await _articleController.Update(new UpdateArticleCommand());
            var result = response as Response<Unit>;
            Assert.IsType<Unit>(result.Body);
        }
        
        [Fact]
        public async void Update_UnExistingArticle_ShouldReturnUnit()
        {
            var response = await _articleController.Update(new UpdateArticleCommand{ArticleId = 12, Author = "test",ArticleContent = "test",PublishDate = "asd",StarCount = 5});
            var result = response as Response<Unit>;
            Assert.IsType<Unit>(result.Body);
        }

    }
}