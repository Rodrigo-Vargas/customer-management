using api.ViewModels.Login;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace api.tests
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpClient;

        public AuthControllerTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task UserLoginSucessfully()
        {
            var credentials = new LoginRequest
            {
                Email = "admin@app.com",
                Password = "admin@123"
            };

            var content = new StringContent(credentials.ToString(), Encoding.UTF8, "application/json");

            var loginResponse = await httpClient.PostAsync("/api/auth/login", content);
            
            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResponse>(loginResponseContent);

            Assert.Equal(credentials.Email, loginResult.Email);
            Assert.False(string.IsNullOrWhiteSpace(loginResult.Token));
        }
    }
}
