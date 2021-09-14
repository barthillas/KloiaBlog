using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ReviewService.Api.Helpers;

namespace ReviewService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().RefreshDatabase().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
