using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using ArticleService.Infrastructure.Context;
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
                if (!appContext.Articles.Any())
                {
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                }
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