using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace api.Services
{
   public interface IUserService
   {
      Task<bool> CheckUserIsAdministratorAsync(string userName);
   }

   public class UserService : IUserService
   {
      private readonly ApiDbContext _context;
      private readonly ILogger<UserService> _logger;
      private readonly RoleManager<IdentityRole> _roleManager;
      private readonly UserManager<User> _userManager;

      public UserService(ApiDbContext context, ILogger<UserService> logger, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
      {
         _logger = logger;
         _roleManager = roleManager;
         _userManager = userManager;
      }

      public async Task<bool> CheckUserIsAdministratorAsync(string userName)
      {

         var user = await _userManager.FindByNameAsync(userName);
         var roles = await _userManager.GetRolesAsync(user);
         
         return roles.Contains("Administrator");
      }
   }
}