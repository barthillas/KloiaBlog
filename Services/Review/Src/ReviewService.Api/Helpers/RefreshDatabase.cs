using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReviewService.Infrastructure.Context;

namespace ReviewService.Api.Helpers
{
    public static class RefreshDatabaseManager
    {
        public static IHost RefreshDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ReviewDbContext>();
            try
            {
                if (!appContext.Reviews.Any())
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