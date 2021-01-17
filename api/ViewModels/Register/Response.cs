using System.Collections.Generic;

namespace api.ViewModels.Register
{
    public class RegisterResponse
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}