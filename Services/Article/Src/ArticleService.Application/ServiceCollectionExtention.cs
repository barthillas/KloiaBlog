using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Data.CQRS;

namespace ArticleService.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services)
        { 
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services.AddSingleton<ICommandSender, CommandSender>();
            return services;
        }
    }
}