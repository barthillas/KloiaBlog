using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.CQRS
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCqrsExtension(this IServiceCollection services)
        {
           
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
            services.AddSingleton<ICommandSender, CommandSender>();
            return services;
        }
    }
}