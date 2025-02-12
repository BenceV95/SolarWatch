using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SolarWatch.Context;
using SolarWatch.Models;

namespace SolarWatch.Services
{
    public class SolarDataProvider : ISolarDataProvider
    {
        private readonly ILogger<SolarDataProvider> _logger;
        private readonly HttpClient _client = new HttpClient();
        private readonly AppSettings _appSettings;

        public SolarDataProvider(ILogger<SolarDataProvider> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }
        public async Task<string> GetSunriseSunsetAsync(DateOnly date, GeocodingData location)
        {
            // process location here first 
            var lat = location.Latitude;
            var lon = location.Longitude;
            var formattedDate = $"{date.Year}-{date.Month}-{date.Day}";
            var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={formattedDate}&formatted=0";

            //Debug.WriteLine("Hello, this is the debug for testing.\n" + url);

            _logger.LogInformation("Calling Sunrise-Sunset API with url:\n {url}", url);

            var response = await _client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetLocationFromNameAsync(string location)
        {

            // http://api.openweathermap.org/geo/1.0/direct?q={city name},{state code},{country code}&limit={limit}&appid={API key}
            var url = $"http://api.openweathermap.org/geo/1.0/direct?q={location}&limit=1&appid={_appSettings.OpenWeatherMapApiKey}";

            _logger.LogInformation("Calling OpenWeatherMap API with url:\n {url}", url);

            var response = await _client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();


        }
    }
}
