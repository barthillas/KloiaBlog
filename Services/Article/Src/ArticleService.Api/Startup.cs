using ApiBase.HeathCheck;
using ApiBase.Middlewares;
using ArticleService.Api.Helpers;
using ArticleService.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNet.OData.Extensions;
using ArticleService.Infrastructure.Context;
using Data.UnitOfWork;

namespace ArticleService.Api
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Article.Api", Version = "v1" });
            });

            services.AddArticleDbContext(Configuration);
            services.AddHealthChecks().AddDbContextCheck<ArticleDbContext>();
            
            services.AddOData();
            services.AddODataClient(Configuration);
            
            services.AddCqrs();
            
            services.AddSingleton(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            
            services.AddMvc(option => option.EnableEndpointRouting = false);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Article.Api v1"));
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMvc(p =>
            {
                p.EnableDependencyInjection();
                p.Select().Expand().Count().Filter().OrderBy().MaxTop(100).SkipToken().Build();
                p.MapODataServiceRoute("odata", "odata", Edm.GetEdmModel());
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
