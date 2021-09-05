using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Data.CQRS;
using Microsoft.Extensions.Configuration;

namespace ArticleService.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services)
        { 
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}