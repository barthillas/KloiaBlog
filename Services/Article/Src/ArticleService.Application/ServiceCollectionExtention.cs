using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ArticleService.Abstraction.Command;
using ArticleService.Abstraction.Validation;
using ArticleService.Application.Processors;
using Autofac;
using Autofac.Core.Activators.Reflection;
using Autofac.Extensions.DependencyInjection;
using Data.CQRS;
using FluentValidation;

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