using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch;
using Microsoft.AspNetCore.Hosting;
using SolarWatch.Context;
using SolarWatch.Contracts;
using SolarWatch.Controllers;
using Assert = Xunit.Assert;

namespace SolarWatchTests
{
    [Collection("IntegrationTests")]
    public class IntegrationTestForAuth : IDisposable
    {
        private readonly string _dbName = Guid.NewGuid().ToString();
        public HttpClient Client { get; }
        public string Token { get; private set; }

        public IntegrationTestForAuth()
        {
            var factory = new WebApplicationFactory<Program>();

            var webAppFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    /*
                    // Remove all DbContextOptions registrations
                    var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<SolarWatchApiContext>)).ToList();
                    foreach (var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }
                    */
                    // Remove any existing DbContext registration
                    var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(SolarWatchApiContext));
                    if (dbContextDescriptor != null)
                    {
                        services.Remove(dbContextDescriptor);
                    }

                    // Add the in-memory database
                    services.AddDbContext<SolarWatchApiContext>(options =>
                        options.UseInMemoryDatabase(_dbName));

                    // Ensure database is created in a separate scope
                    using var scope = services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<SolarWatchApiContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                });
            });

            Client = webAppFactory.CreateClient();
            RegisterTestUser().GetAwaiter().GetResult(); // register a new user before tests for [Authorize] to work
        }

        private async Task RegisterTestUser()
        {
            var newUser = new RegistrationRequest("testuser@test.com", "testuser", "testuser");
            var response = await Client.PostAsJsonAsync("/Auth/Register", newUser);
            response.EnsureSuccessStatusCode();

            var login = await Client.PostAsJsonAsync("/Auth/Login", new AuthController.AuthRequest("testuser@test.com", "testuser"));
            var token = await login.Content.ReadFromJsonAsync<AuthController.AuthResponse>();
            Token = token.Token;

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        public void Dispose() => Client.Dispose();
    }

    public class IntegrationTests : IClassFixture<IntegrationTestForAuth>
    {

        private readonly HttpClient _client;

        public IntegrationTests(IntegrationTestForAuth setupAuth)
        {
            _client = setupAuth.Client;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", setupAuth.Token);
        }

        [Fact]
        public async Task LoginTest()
        {

            var testEndpoint = await _client.GetAsync("/Auth/testUser");

            Assert.Equal(HttpStatusCode.OK, testEndpoint.StatusCode);
        }

        [Fact]
        public async Task GetGeocodingData_ValidLocation_ReturnsOk()
        {
            // Arrange
            var location = "London";

            // Act
            var response = await _client.GetAsync($"/SolarWatch/GeocodingData?location={location}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain(location);
        }

        [Fact]
        public async Task GetGeocodingData_InvalidLocation_ReturnsNotFound()
        {
            // Arrange
            var location = "UnknownLocation";

            // Act
            var response = await _client.GetAsync($"/SolarWatch/GeocodingData?location={location}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSolarData_ValidData_ReturnsOk()
        {
            // Arrange
            var location = "London";
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/SolarWatch/SolarData?date={date}&location={location}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain(date);
        }

        [Fact]
        public async Task GetSolarData_InvalidData_ReturnsNotFound()
        {
            // Arrange
            var location = "UnknownLocation";
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/SolarWatch/SolarData?date={date}&location={location}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
