using SolarWatch.Models;
using System.Text.Json;

namespace SolarWatch.Services
{
    public class JsonProcessor : IJsonProcessor
    {
        public SolarData ProcessJsonForSolarData(string solarData)
        {
            JsonDocument json = JsonDocument.Parse(solarData);
            JsonElement root = json.RootElement;
            JsonElement main = root.GetProperty("results");

            string date = main.GetProperty("solar_noon").GetString();
            DateTime dt = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.RoundtripKind);

            SolarData solarInfo = new SolarData()
            {
                Date = DateOnly.FromDateTime(dt),
                Sunrise = main.GetProperty("sunrise").GetDateTime(),
                Sunset = main.GetProperty("sunset").GetDateTime(),
                SolarNoon = main.GetProperty("solar_noon").GetDateTime(),
                DayLength = main.GetProperty("day_length").GetInt32(),
                CivilTwilightBegin = main.GetProperty("civil_twilight_begin").GetDateTime(),
                CivilTwilightEnd = main.GetProperty("civil_twilight_end").GetDateTime(),
                NauticalTwilightBegin = main.GetProperty("nautical_twilight_begin").GetDateTime(),
                NauticalTwilightEnd = main.GetProperty("nautical_twilight_end").GetDateTime(),
                AstronomicalTwilightBegin = main.GetProperty("astronomical_twilight_begin").GetDateTime(),
                AstronomicalTwilightEnd = main.GetProperty("astronomical_twilight_end").GetDateTime(),
                TimeZoneID = root.GetProperty("tzid").GetString()
            };

            solarInfo.ConvertToUtc();

            return solarInfo;
        }

        public GeocodingData ProcessJsonForGeocodingData(string geocodingData)
        {
            JsonDocument json = JsonDocument.Parse(geocodingData);
            JsonElement root = json.RootElement[0];

            root.TryGetProperty("state",out var state);

            GeocodingData geoInfo = new GeocodingData()
            {
                Name = root.GetProperty("name").GetString(),
                Country = root.GetProperty("country").GetString(),
                Latitude = root.GetProperty("lat").GetDouble(),
                Longitude = root.GetProperty("lon").GetDouble(),
                State = state.ToString()

            };

            return geoInfo;
        }
    }
}
