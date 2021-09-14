using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReviewService.Api;
using ReviewService.Application;
using ReviewService.Infrastructure.Context;
using Xunit;

namespace ReviewService.UnitTest.Tests
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
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ReviewDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                    services.AddDbContext<ReviewDbContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                    services.AddCqrs();
                });
            });
            
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("InboundRequest","test");
        }
        
        [Theory]
        [InlineData("api/Review/Create")]
        public async Task Create_Review_ShouldReturn500(string url)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var multiContent = new MultipartFormDataContent();
            multiContent.Add(new StringContent( "ReviewContent"), "ReviewContent");
            multiContent.Add(new StringContent( "Reviewer"), "Reviewer");
            multiContent.Add(new StringContent( "2"), "ArticleId");

            var response = await _client.PostAsync(url, multiContent);
            
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
        
        [Theory]
        [InlineData("api/Review/Create")]
        public async Task Review_Post_ValidationError(string url)
        {
            var response = await _client.PostAsync(url, new MultipartFormDataContent());
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData("api/Review/Update")]
        public async Task UpdateReview_ShouldReturn_OK(string url)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var multiContent = new MultipartFormDataContent();
            
            multiContent.Add(new StringContent( "3"), "ReviewId");
            multiContent.Add(new StringContent( "ReviewContent"), "ReviewContent");
            multiContent.Add(new StringContent( "Reviewer"), "Reviewer");
            multiContent.Add(new StringContent( "5"), "ArticleId");

            var response = await _client.PutAsync(url, multiContent);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Theory]
        [InlineData("api/Review/Update")]
        public async Task UpdateReview_ShouldReturn_BadRequest(string url)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            var multiContent = new MultipartFormDataContent();
            
            multiContent.Add(new StringContent( "100000"), "ReviewId");

            var response = await _client.PutAsync(url, multiContent);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("/odata")]
        [InlineData("/odata/reviews")]
        [InlineData("/odata/reviews(1)")]
        public async Task Get(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}