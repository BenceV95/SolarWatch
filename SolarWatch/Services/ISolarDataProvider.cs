
using SolarWatch.Models;

namespace SolarWatch.Services
{
    public interface ISolarDataProvider
    {
        Task<string> GetSunriseSunsetAsync(DateOnly date, GeocodingData location);
        Task<string> GetLocationFromNameAsync(string location);

    }
}
