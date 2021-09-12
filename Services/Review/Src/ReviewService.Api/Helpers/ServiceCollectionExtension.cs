using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReviewService.Infrastructure.Context;
using Simple.OData.Client;

namespace ReviewService.Api.Helpers
{
    public static class ServiceCollectionExtension
    {
        public static void AddReviewDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];
            var isDockerEnv = Environment.GetEnvironmentVariable("ISDOCKERENV");
            if (!string.IsNullOrEmpty(isDockerEnv))
            {
                connectionString = configuration["ConnectionStrings:DockerDefaultConnection"];
            }

            services.AddDbContext<ReviewDbContext>(opt =>
                opt.UseSqlServer(connectionString,
                    options => options.MigrationsAssembly(Assembly.GetAssembly(typeof(ReviewDbContext))?.FullName))
            );
        }
        
        public static void AddODataClient(this IServiceCollection services, IConfiguration configuration)
        {
            var odataUrl = configuration["ArticleOdataHost:Url"];
            if (Environment.GetEnvironmentVariable("ISDOCKERENV") != null)
            {
                odataUrl = configuration["ArticleOdataHost:DockerUrl"];
            }

            var oDataSettings = new ODataClientSettings(new Uri(odataUrl))
            {
                IgnoreResourceNotFoundException = true,
                OnTrace = (x, y) => Console.WriteLine(string.Format(x, y)),
            };
            oDataSettings.BeforeRequest += delegate(HttpRequestMessage message)
            {
                message.Headers.Add("InboundRequest", Assembly.GetExecutingAssembly().FullName);
            };

            services.AddSingleton(new ODataClient(oDataSettings));
        }
    }
}