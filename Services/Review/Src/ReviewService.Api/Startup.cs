using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Net.Http;
using System.Reflection;
using ApiBase.HeathCheck;
using ApiBase.Middlewares;
using Data.CQRS;
using Data.UnitOfWork;
using FluentValidation.AspNetCore;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.EntityFrameworkCore;
using ReviewService.Abstraction.Validation;
using ReviewService.Api.Helpers;
using ReviewService.Application;
using ReviewService.Infrastructure.Context;
using Simple.OData.Client;

namespace ReviewService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Review.Api", Version = "v1" });
            });
            
            services.AddReviewDbContext(Configuration);
            services.AddHealthChecks().AddDbContextCheck<ReviewDbContext>();
            
            services.AddOData();
            services.AddODataClient(Configuration);
            
            services.AddCqrs();
            services.AddSingleton(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateReviewCommandValidator>());;
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Review.Api v1"));
            }
        
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMvc(p =>
            {
                p.EnableDependencyInjection();
                p.Select().Expand().Count().Filter().OrderBy().MaxTop(100).SkipToken().Build();
                p.MapODataServiceRoute("odata", "odata", EdmReview.GetEdmModel());
            });
            app.UseRouting();

            app.UseAuthorization();
            app.ConfigureHealthCheck();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
