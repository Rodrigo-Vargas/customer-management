using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.ViewModels.Register
{
    public class RegisterRequest
    {
        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Required]
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}