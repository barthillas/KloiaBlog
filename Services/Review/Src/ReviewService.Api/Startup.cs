using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiBase.Middlewares;
using Data.UnitOfWork;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.EntityFrameworkCore;
using ReviewService.Api.Helpers;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;

namespace ReviewService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Article.Api", Version = "v1" });
            });
            
            
            services.AddDbContext<ReviewDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddSingleton(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddOData();
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Article.Api v1"));
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
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
