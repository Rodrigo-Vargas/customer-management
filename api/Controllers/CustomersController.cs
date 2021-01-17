using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
   [Route("api/customers")]
   public class CustomersController : Controller
   {
      private readonly ApiDbContext _context;
      private readonly IUserService _userService;

      public CustomersController(ApiDbContext context, IUserService userService)
      {
         _context = context;
         _userService = userService;
      }

      [HttpGet]
      [Authorize]
      public async Task<IEnumerable<Customer>> GetAsync()
      {
         var user = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

         if (await _userService.CheckUserIsAdministratorAsync(User.Identity.Name))
            return _context.Customers.OrderBy((o)=> o.Name);

         return _context.Customers.Where(x => x.User.Id == user.Id).OrderBy((o)=> o.Name);
      }
   }
}