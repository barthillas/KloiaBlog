using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Data.CQRS;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ReviewService.Abstraction.Command;
using ReviewService.Api;
using ReviewService.Application;
using ReviewService.Infrastructure.Context;
using Xunit;

namespace ReviewService.UnitTest.Tests
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
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ReviewDbContext>));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }
                        services.AddDbContext<ReviewDbContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                        services.AddCqrs();
                        services.AddCqrsExtension();
                    });
                });

            _client = appFactory.CreateClient();
            _client.DefaultRequestHeaders.Add("InboundRequest","test");

        }
        
        [Fact]
        public async Task Get_ReturnsReview_WhenReviewExistsInTheDatabase()
        {
            var request = new CreateReviewCommand { ReviewContent =" ReviewContent", Reviewer = "Reviewer", ArticleId = 12,  };
            var aasd = await CreateReview(request);
            var response = await _client.GetAsync("api/Reviews");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task<HttpResponseMessage> CreateReview(CreateReviewCommand request)
        {
            var requestSerializeObject = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestSerializeObject, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await _client.PostAsync("/api/Review/Create", content);
        }
        
     
    }
}