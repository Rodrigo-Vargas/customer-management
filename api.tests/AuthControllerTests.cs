using api.Models;
using api.ViewModels.Login;
using api.ViewModels.Register;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly ApiDbContext repository;

        public AuthControllerTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateClient();

            var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
            optionsBuilder.UseInMemoryDatabase("MyInMemoryDatabseName"); 
            repository = new ApiDbContext(optionsBuilder.Options);
        }

         [Fact]
        public async Task UserRegistrationSucessfully()
        {   
            var allUsers = repository.Users;
            foreach (var user in allUsers)
            {
                repository.Users.Remove(user);
            }

            repository.SaveChanges();

            var credentials = new RegisterRequest
            {
                Email = "admin@app.com",
                Password = "Admin@123"
            };

            var content = new StringContent(credentials.ToString(), Encoding.UTF8, "application/json");

            var registerResponse = await httpClient.PostAsync("api/auth/login", content);
            
            var registerResponseContent = await registerResponse.Content.ReadAsStringAsync();
            var registerResult = JsonSerializer.Deserialize<RegisterResponse>(registerResponseContent);

            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);
        }

        [Fact]
        public async Task UserLoginSucessfully()
        {
            var credentials = new LoginRequest
            {
                Email = "admin@app.com",
                Password = "Admin@123"
            };

            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

            var loginResponse = await httpClient.PostAsync("api/auth/login", content);
            Console.WriteLine(loginResponse);
            
            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResponse>(loginResponseContent);

            Assert.Equal(credentials.Email, loginResult.Email);
            Assert.False(string.IsNullOrWhiteSpace(loginResult.Token));
        }
    }
}
