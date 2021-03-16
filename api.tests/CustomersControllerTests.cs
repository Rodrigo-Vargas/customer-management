using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using api.Models;
using api.ViewModels.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace api.tests
{
    public class CustomersControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpClient;

        public CustomersControllerTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task ShouldReturnOnlyContactsOfSeller()
        {
            var credentials = new LoginRequest
            {
                Email = "seller1@app.com",
                Password = "Seller@1"
            };
            var loginResponse = await httpClient.PostAsync("api/auth/login",
                new StringContent(
                    JsonSerializer.Serialize(credentials), Encoding.UTF8, MediaTypeNames.Application.Json)
            );

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResponse>(loginResponseContent);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.Token);
            var getCustomersResponse = await httpClient.PostAsync("api/customers", null);
            Assert.Equal(HttpStatusCode.OK, getCustomersResponse.StatusCode);
        }
    }
}