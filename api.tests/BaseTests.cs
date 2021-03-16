using System.Net.Http;
using api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace api.tests
{
   public class BaseTests : IClassFixture<WebApplicationFactory<Startup>>
   {
      private readonly HttpClient httpClient;
      private readonly ApiDbContext repository;

      public BaseTests(WebApplicationFactory<Startup> factory)
      {
         httpClient = factory.CreateClient();

         var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
         optionsBuilder.UseInMemoryDatabase("CustomerManagementMemoryDatabase"); 
         repository = new ApiDbContext(optionsBuilder.Options);
      }
   }

   public class TestFixture
   {
       public ServiceProvider ServiceProvider { get; private set; }
        public TestFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlite("Data Source=customer_management.db");
            });

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }    
   }
}