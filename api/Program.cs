using System.IO;
using api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace api
{
   public class Program
   {
      public static void Main(string[] args)
      {
         var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

         var host = CreateHostBuilder(config, args).Build();

         using (var scope = host.Services.CreateScope())
         {
            var dbContext = scope.ServiceProvider.GetService<ApiDbContext>();
            dbContext.Database.Migrate();

            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
               // Seed the database in development mode
               var dbInitializer = scope.ServiceProvider.GetRequiredService<Models.IDefaultDbContextInitializer>();
               dbInitializer.Seed().GetAwaiter().GetResult();
            }
         }
         
         host.Run();
      }

      public static IHostBuilder CreateHostBuilder(IConfigurationRoot config, string[] args) =>
         Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
               webBuilder
               .UseUrls(config["serverBindingUrl"])
               .UseStartup<Startup>();
            });
    }
}
