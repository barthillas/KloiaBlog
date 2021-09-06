using System.Reflection;
using Data.CQRS;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ReviewService.Application
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