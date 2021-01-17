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

         if (!_context.Genders.Any())
         {
            _context.Genders.Add(new Gender() { Name = "Masculine" });
            _context.Genders.Add(new Gender() { Name = "Feminine" });
         }

         _context.SaveChanges();
      }
    }

    public interface IDefaultDbContextInitializer
    {
        bool EnsureCreated();
        Task Seed();
    }
}
