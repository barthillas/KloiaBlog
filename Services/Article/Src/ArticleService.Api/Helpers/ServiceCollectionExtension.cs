using System;
using System.Net.Http;
using System.Reflection;
using ArticleService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simple.OData.Client;

namespace ArticleService.Api.Helpers
{
    public static class ServiceCollectionExtension
    {
        public static void AddArticleDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];
            var isDockerEnv = Environment.GetEnvironmentVariable("ISDOCKERENV");
            if (!string.IsNullOrEmpty(isDockerEnv))
            {
                connectionString = configuration["ConnectionStrings:DockerDefaultConnection"];
            }
            Console.WriteLine("connectionString: "+connectionString);
            services.AddDbContext<ArticleDbContext>(opt =>
                opt.UseSqlServer(connectionString,
                    options => options.MigrationsAssembly(Assembly.GetAssembly(typeof(ArticleDbContext))?.FullName))
            );
        }
        
        public static void AddODataClient(this IServiceCollection services, IConfiguration configuration)
        {
            var odataUrl = configuration["ReviewOdataHost:Url"];
            if (Environment.GetEnvironmentVariable("ISDOCKERENV") != null)
            {
                odataUrl = configuration["ReviewOdataHost:DockerUrl"];
            }
            Console.WriteLine("odataUrl: "+odataUrl);
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