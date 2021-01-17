using api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace api
{
   public class Program
   {
      public static void Main(string[] args)
      {
         var host = CreateHostBuilder(args).Build();

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

      public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
               webBuilder.UseStartup<Startup>();
            });
    }
}
