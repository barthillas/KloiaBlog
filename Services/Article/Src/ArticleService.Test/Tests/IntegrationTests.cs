using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ArticleService.Abstraction.Command;
using ArticleService.Api;
using ArticleService.Application;
using ArticleService.Domain.Entities;
using ArticleService.Infrastructure.Context;
using Data.CQRS;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ThrowawayDb;
using Xunit;
using Xunit.Abstractions;

namespace ArticleService.UnitTest.Controllers.Tests
{
    public class IntegrationTest : TestBase.TestBase
    {
        private readonly HttpClient _client;
    
        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ArticleDbContext>));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }
                        services.AddDbContext<ArticleDbContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                        services.AddCqrs();
                        services.AddCqrsExtension();
                    });
                });

            _client = appFactory.CreateClient();
            _client.DefaultRequestHeaders.Add("InboundRequest","test");

        }
        
        [Fact]
        public async Task Get_ReturnsArticle_WhenArticleExistsInTheDatabase()
        {
            var request = new CreateArticleCommand { ArticleContent =" ArticleContent", Author ="Author", PublishDate = "12.12.2012", StarCount = 5, Title = "Title" };
            var aasd = await CreateArticle(request);
            var response = await _client.GetAsync("api/Articles");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task<HttpResponseMessage> CreateArticle(CreateArticleCommand request)
        {
            var requestSerializeObject = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestSerializeObject, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await _client.PostAsync("/api/Article/Create", content);
        }
        
        [Fact]
        public async Task Get_Articles_ReturnsStatusOk()
        {
            var response = await _client.GetAsync("api/articles");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}