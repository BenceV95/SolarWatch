using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Repositories;
using SolarWatch.Services;
using Assert = NUnit.Framework.Assert;

namespace SolarWatchTests
{
    [TestFixture]
    public class SolarWatchControllerTests
    {
        private SolarWatchController _controller;
        private ILogger<SolarWatchController> _logger;
        private ISolarDataProvider _provider;
        private IJsonProcessor _processor;
        private ISolarWatchRepository _repository;
        private SolarDataService _solarDataService;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<SolarWatchController>>();
            _provider = Substitute.For<ISolarDataProvider>();
            _processor = Substitute.For<IJsonProcessor>();
            _repository = Substitute.For<ISolarWatchRepository>();

            _solarDataService = new SolarDataService(_provider,_processor,_repository);
            _controller = new SolarWatchController(_logger, _solarDataService);
        }

        [Test]
        public async Task GetGeocodingData_LocationExistsInRepository_ReturnsOk()
        {
            // Arrange
            var location = "TestLocation";
            var geocodingData = new GeocodingData { Id = 1, Name = location };
            _repository.GetGeocodingDataByNameAsync(location).Returns(geocodingData);

            // Act
            var result = await _controller.GetGeocodingData(location);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(geocodingData, okResult?.Value);
        }


        [Test]
        public async Task GetGeocodingData_LocationNotFound_ReturnsNotFound()
        {
            // Arrange
            var location = "UnknownLocation";
            _repository.GetGeocodingDataByNameAsync(location).Returns(Task.FromResult<GeocodingData?>(null as GeocodingData));
            _provider.GetLocationFromNameAsync(location).Returns(Task.FromResult(""));

            // Act
            var result = await _controller.GetGeocodingData(location); // should throw an exception but during testing does not get triggered

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetGeocodingData_LocationFoundInProvider_ReturnsOk()
        {
            // Arrange
            var location = "NewLocation";
            var locationData = "{\"lat\": 123.45, \"lon\": 67.89}";
            var geocodingData = new GeocodingData { Id = 1, Name = location };
            _repository.GetGeocodingDataByNameAsync(location).Returns(Task.FromResult<GeocodingData?>(null as GeocodingData));
            _provider.GetLocationFromNameAsync(location).Returns(Task.FromResult(locationData));
            _processor.ProcessJsonForGeocodingData(locationData).Returns(geocodingData);

            // Act
            var result = await _controller.GetGeocodingData(location);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(geocodingData, Is.EqualTo(okResult?.Value));
        }




        [Test]
        public async Task GetSolarData_LocationExistsInRepository_ReturnsOk()
        {
            // Arrange
            var location = "TestLocation";
            var date = DateOnly.FromDateTime(DateTime.Now);
            var geocodingData = new GeocodingData { Id = 1, Name = location };
            var solarData = new SolarData { Id = 1, GeocodingDataId = geocodingData.Id, Date = date };
            _repository.GetGeocodingDataByNameAsync(location).Returns(Task.FromResult<GeocodingData?>(geocodingData));
            _repository.GetSolarDataAsync(date, geocodingData.Id).Returns(Task.FromResult<SolarData?>(solarData));

            // Act
            var result = await _controller.GetSolarData(date, location);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(solarData, Is.EqualTo(okResult?.Value));
        }

        [Test]
        public async Task GetSolarData_LocationNotFound_ReturnsNotFound()
        {
            // Arrange
            var location = "UnknownLocation";
            var date = DateOnly.FromDateTime(DateTime.Now);
            _repository.GetGeocodingDataByNameAsync(location).Returns(Task.FromResult<GeocodingData?>(null as GeocodingData));
            _provider.GetLocationFromNameAsync(location).Returns(Task.FromResult(""));

            // Act
            var result = await _controller.GetSolarData(date, location);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        [Test]
        public async Task DeleteCity_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _controller.DeleteCity(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteSolarData_ValidId_ReturnsOk()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _controller.DeleteSolarData(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
