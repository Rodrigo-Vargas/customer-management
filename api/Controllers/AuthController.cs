using api.ViewModels.Login;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        [HttpPost("login")]
        public User Login(LoginRequest request)
        {
            return new User()
            {
                Email = "admin@ronaldo.com.br"
            };
        }
    }

    public class User
    {
        public string Email { get; set; }
    }
}