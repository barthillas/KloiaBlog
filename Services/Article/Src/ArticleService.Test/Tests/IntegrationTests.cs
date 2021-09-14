using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArticleService.Api;
using ArticleService.Application;
using ArticleService.Infrastructure.Context;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ArticleService.UnitTest.Tests
{
    public class IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        private readonly HttpClient _client;
        public IntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _factory= factory.WithWebHostBuilder(builder =>
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
                });
            });
            
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("InboundRequest","test");
        }
        
        [Theory]
        [InlineData("api/Article/Create")]
        public async Task Create_Article_ShouldSuccess(string url)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var multiContent = new MultipartFormDataContent();
            multiContent.Add(new StringContent( "ArticleContent"), "ArticleContent");
            multiContent.Add(new StringContent( "Author"), "Author");
            multiContent.Add(new StringContent( "12.12.2012"), "PublishDate");
            multiContent.Add(new StringContent( "5"), "StarCount");
            multiContent.Add(new StringContent( "Title"), "Title");

            var response = await _client.PostAsync(url, multiContent);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Theory]
        [InlineData("api/Article/Create")]
        public async Task Article_Post_ValidationError(string url)
        {
            var response = await _client.PostAsync(url, new MultipartFormDataContent());
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData("api/Article/Update")]
        public async Task UpdateArticle_ShouldReturn_OK(string url)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var multiContent = new MultipartFormDataContent();
            
            multiContent.Add(new StringContent( "1"), "ArticleId");
            multiContent.Add(new StringContent( "ArticleContent"), "ArticleContent");
            multiContent.Add(new StringContent( "Author"), "Author");
            multiContent.Add(new StringContent( "12.12.2012"), "PublishDate");
            multiContent.Add(new StringContent( "5"), "StarCount");
            multiContent.Add(new StringContent( "Title"), "Title");

            var response = await _client.PutAsync(url, multiContent);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Theory]
        [InlineData("api/Article/Update")]
        public async Task UpdateArticle_ShouldReturn_BadRequest(string url)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var multiContent = new MultipartFormDataContent();
            
            multiContent.Add(new StringContent( "100000"), "ArticleId");

            var response = await _client.PutAsync(url, multiContent);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData("api/Article/Update")]
        public async Task UpdateUnExistArticle_ShouldReturn_UnprocessableEntity(string url)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var multiContent = new MultipartFormDataContent();
            
            multiContent.Add(new StringContent( "1111111"), "ArticleId");
            multiContent.Add(new StringContent( "ArticleContent"), "ArticleContent");
            multiContent.Add(new StringContent( "Author"), "Author");
            multiContent.Add(new StringContent( "12.12.2012"), "PublishDate");
            multiContent.Add(new StringContent( "5"), "StarCount");
            multiContent.Add(new StringContent( "Title"), "Title");

            var response = await _client.PutAsync(url, multiContent);
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }


        [Theory]
        [InlineData("/odata")]
        [InlineData("/odata/articles")]
        [InlineData("/odata/articles(1)")]
        public async Task Get(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}