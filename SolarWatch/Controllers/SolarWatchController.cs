using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using SolarWatch.Services;
using SolarWatch.Models;

namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolarWatchController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISolarDataService _solarDataService;

        public SolarWatchController(ILogger<SolarWatchController> logger, ISolarDataService solarDataService)
        {
            _logger = logger;
            _solarDataService = solarDataService;
        }

        [HttpGet("GeocodingData"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<GeocodingData>> GetGeocodingData([Required] string location)
        {
            try
            {
                var data = await _solarDataService.GetGeocodingDataAsync(location);
                return Ok(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in {MethodName}: {ErrorMessage}", nameof(GetGeocodingData), e.Message);
                return e is LocationNotFoundException ? NotFound(e.Message) : StatusCode(500, "Error getting the data.\n" + e.Message);
            }
        }


        [HttpGet("SolarData"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<SolarData>> GetSolarData([Required] DateOnly date, [Required] string location)
        {
            try
            {
                var data = await _solarDataService.GetSolarDataAsync(date, location);
                return Ok(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in {MethodName}: {ErrorMessage}", nameof(GetGeocodingData), e.Message);

                return e is LocationNotFoundException ? NotFound(e.Message) : StatusCode(500, "Error getting the data.\n" + e.Message);
            }
        }


        [HttpDelete("deleteCity"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCity([FromQuery,Required] int id)
        {
            try
            {
                return Ok(await _solarDataService.DeleteCity(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in {MethodName}: {ErrorMessage}", nameof(GetGeocodingData), e.Message);
                return BadRequest($"Unable to delete city data with id: {id}.");
            }
        }


        [HttpDelete("deleteSolarData"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteSolarData([FromQuery, Required] int id)
        {
            try
            {
                return Ok(await _solarDataService.DeleteSolarData(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in {MethodName}: {ErrorMessage}", nameof(GetGeocodingData), e.Message);
                return BadRequest($"Unable to delete solar data with id: {id}.");
            }
        }
    }
}
