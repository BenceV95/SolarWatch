using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch;
using Microsoft.AspNetCore.Hosting;
using SolarWatch.Context;

namespace SolarWatchTests
{
    [Collection("IntegrationTests")]
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly string _dbName = Guid.NewGuid().ToString();
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            var webAppFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.ConfigureServices(services =>
                {
                    // Remove all DbContextOptions registrations
                    var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<SolarWatchApiContext>)).ToList();
                    foreach (var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }

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

            _client = webAppFactory.CreateClient();
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
