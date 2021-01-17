using Microsoft.AspNetCore.Identity;
using System;
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

         SeedCustomers();
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

      private void SeedCustomers()
      {
          if (_context.Customers.Any())
            foreach (var u in _context.Customers)
               _context.Remove(u);

         CreateCustomer("Maurício", "(11) 95429999", "Masculine", "Porto Alegre", "Rio Grande do Sul", "VIP", new DateTime(2016, 9, 10), "seller2@app.com");
         CreateCustomer("Carla", "(53) 94569999", "Feminine", "Porto Alegre", "Rio Grande do Sul", "VIP",  new DateTime(2015, 10, 10), "seller1@app.com");
         CreateCustomer("Maria", "(64) 94518888", "Feminine", "Porto Alegre", "Rio Grande do Sul", "Sporadic",  new DateTime(2013, 10, 12), "seller1@app.com");
         CreateCustomer("Douglas", "(51) 12455555", "Masculine", "Porto Alegre", "Rio Grande do Sul", "Regular",  new DateTime(2016, 5, 5), "seller1@app.com");
         CreateCustomer("Marta", "(51) 45788888", "Feminine", "Porto Alegre", "Rio Grande do Sul", "Regular",  new DateTime(2016, 8, 8), "seller2@app.com");
      }

      private void CreateCustomer(string name, string phone, string gender, string city, string region, string classification, DateTime lastPurchase, string userEmail)
      {
         _context.Customers.Add(new Customer() {
            Name = name,
            Phone = phone,
            Gender = _context.Genders.FirstOrDefault(x => x.Name == gender),
            Region = _context.Regions.FirstOrDefault(x => x.Name == region),
            City = _context.Cities.FirstOrDefault(x => x.Name == city),
            Classification = _context.Classifications.FirstOrDefault(x => x.Name == classification),
            LastPurchase = lastPurchase,
            User = _context.Users.FirstOrDefault(x => x.Email == userEmail)
         });

         _context.SaveChanges();
      }
   }

   public interface IDefaultDbContextInitializer
   {
      bool EnsureCreated();
      Task Seed();
   }
}
