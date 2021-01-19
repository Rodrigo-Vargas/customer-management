using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using api.ViewModels.Login;
using api.ViewModels.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly ApiDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger _logger;

        public AuthController(ApiDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<JwtOptions> jwtOptions, ILoggerFactory loggerFactory)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _logger = loggerFactory.CreateLogger<AuthController>();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
           try
           {
               // Ensure the username and password is valid.
               var user = await _userManager.FindByNameAsync(request.Email);
               if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
               {
                  return BadRequest(new
                  {
                     error = "",
                     error_description = "The username or password is invalid."
                  });
               }

               // Generate and issue a JWT token
               var claims = new[] {
                  new Claim(ClaimTypes.Name, user.Email),
                  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
               };

               var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.key));
               var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

               var token = new JwtSecurityToken(
                  issuer: _jwtOptions.issuer,
                  audience: _jwtOptions.issuer,
                  claims: claims,
                  expires: DateTime.Now.AddMinutes(30),
                  signingCredentials: creds
               );

               return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });

            }
            catch(Exception e)
            {
               _logger.LogInformation("Error on Auth Controller Register request: " + e.Message);
               return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
           try
           {
               if (!ModelState.IsValid)
                  return BadRequest(ModelState);

               var user = new User { Email = model.Email, UserName = model.Email };
               var result = await _userManager.CreateAsync(user, model.Password);
               
               if (!result.Succeeded)
                  return BadRequest(new RegisterResponse
                  {
                     Errors = result.Errors.Select(x => x.Description)
                  });

               var applicationRole = await _roleManager.FindByNameAsync(model.Role);
               if (applicationRole != null)
                  await _userManager.AddToRoleAsync(user, applicationRole.Name);

               return Ok();
           }
           catch(Exception e)
           {
               _logger.LogInformation("Error on Auth Controller Register request: " + e.Message);
               return BadRequest();
           }
        }
    }
}