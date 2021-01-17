using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
   public class UserHelper
   {
      public static async Task CreateUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, string email, string password, string role)
      {
         if (await userManager.FindByEmailAsync(email) == null)
         {
            var user = new User
            {
               UserName = email,
               Email = email,
               EmailConfirmed = true
            };

            await userManager.CreateAsync(user, password);

            var applicationRole = await roleManager.FindByNameAsync(role);
            if (applicationRole != null)
               await userManager.AddToRoleAsync(user, applicationRole.Name);
         }
      }
   }
}