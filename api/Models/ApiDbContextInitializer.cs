using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
   public class ApiDbContextInitializer : IDefaultDbContextInitializer
   {
      private readonly ApiDbContext _context;
      private readonly UserManager<User> _userManager;
      private readonly RoleManager<IdentityRole> _roleManager;

      public ApiDbContextInitializer(ApiDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
      {
         _userManager = userManager;
         _context = context;
         _roleManager = roleManager;
      }

      public bool EnsureCreated()
      {
         return _context.Database.EnsureCreated();
      }

      public async Task Seed()
      {
         await UserHelper.CreateUserAsync(_userManager, _roleManager, "admin@app.com", "Admin@123", "Administrative");
         await UserHelper.CreateUserAsync(_userManager, _roleManager, "seller1@app.com", "Seller@1", "Seller");
         await UserHelper.CreateUserAsync(_userManager, _roleManager, "seller2@app.com", "Seller@2", "Seller");

         SeedGenders();
         SeedRegions();
         SeedCities();
         SeedClassifications();

         _context.SaveChanges();
      }

      private void SeedGenders()
      {
         if (_context.Genders.Any())
            return;
            
         _context.Genders.Add(new Gender() { Name = "Masculine" });
         _context.Genders.Add(new Gender() { Name = "Feminine" });
      }

      private void SeedRegions()
      {
         if (_context.Regions.Any())
            return;

         _context.Regions.Add(new Region() { Name = "Rio Grande do Sul" });
         _context.Regions.Add(new Region() { Name = "São Paulo" });
         _context.Regions.Add(new Region() { Name = "Curitiba" });
      }

      private void SeedClassifications()
      {
         if (_context.Classifications.Any())
            return;

         _context.Classifications.Add(new Classification() { Name = "VIP" });
         _context.Classifications.Add(new Classification() { Name = "Regular" });
         _context.Classifications.Add(new Classification() { Name = "Sporadic" });
      }

      private void SeedCities()
      {
         if (_context.Cities.Any())
            return;
        
         _context.Cities.Add(new City() { Name = "Porto Alegre", Region = _context.Regions.FirstOrDefault(x => x.Name == "Rio Grande do Sul") });
      }
   }

   public interface IDefaultDbContextInitializer
   {
      bool EnsureCreated();
      Task Seed();
   }
}
