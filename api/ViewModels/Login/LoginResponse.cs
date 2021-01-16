using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.ViewModels.Login
{
    public class LoginResponse
    {
        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}