using SolarWatch.Models;

namespace SolarWatch.Services
{
    public interface IJsonProcessor
    {
        SolarData ProcessJsonForSolarData(string solarData);
        GeocodingData ProcessJsonForGeocodingData(string geocodingData);
    }
}
