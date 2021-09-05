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
            
            services.AddDbContext<ReviewDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddHealthChecks().AddDbContextCheck<ReviewDbContext>();
            services.AddSingleton(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddOData();
            services.AddCqrs();
            services.AddCqrsExtension();
            services.AddMvc(option => option.EnableEndpointRouting = false).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateReviewCommandValidator>());;
            var oDataSettings = new ODataClientSettings(new Uri(Configuration["ArticleOdataHost:Url"]))
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
                p.MapODataServiceRoute("odata", "api", EdmReview.GetEdmModel());
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
