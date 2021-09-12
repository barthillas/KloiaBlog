using System;
using ArticleService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArticleService.Api.Helpers
{
    public static class RefreshDatabaseManager
    {
        public static IHost RefreshDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ArticleDbContext>();
            try
            { 
                appContext.Database.EnsureDeleted();
                appContext.Database.Migrate();
                appContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return host;
        }

    }
}