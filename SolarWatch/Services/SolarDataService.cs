using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace SolarWatch.Services
{
    public class SolarDataService : ISolarDataService
    {
        private readonly ISolarDataProvider _provider;
        private readonly IJsonProcessor _processor;
        private readonly ISolarWatchRepository _repository;

        public SolarDataService(ISolarDataProvider provider, IJsonProcessor processor, ISolarWatchRepository repository)
        {
            _provider = provider;
            _processor = processor;
            _repository = repository;
        }

        public async Task<GeocodingData> GetGeocodingDataAsync(string location)
        {
            var geocodingData = await _repository.GetGeocodingDataByNameAsync(location);
            // save to database if it not found (it may exist under a different name)
            if (geocodingData == null)
            {
                var locationData = await _provider.GetLocationFromNameAsync(location); // fetch the api
                if (string.IsNullOrEmpty(locationData)) throw new LocationNotFoundException("Location not found!"); // api could not find it

                geocodingData = _processor.ProcessJsonForGeocodingData(locationData); // turn json response into model

                // only save to database if we are 100% sure entry is not there - not sure how to do it more efficiently 
                var checkDBForEntry = await _repository.GetGeocodingDataByNameAsync(geocodingData.Name);
                if (checkDBForEntry == null)
                {
                    await _repository.AddGeocodingDataAsync(geocodingData);
                    await _repository.SaveChangesAsync();
                }
                return checkDBForEntry ?? geocodingData;
            }

            return geocodingData;
        }

        public async Task<SolarData> GetSolarDataAsync(DateOnly date, string location)
        {
            var geoData = await GetGeocodingDataAsync(location);

            var processedSolarData = await _repository.GetSolarDataAsync(date, geoData.Id);

            if (processedSolarData == null)
            {
                var solarData = await _provider.GetSunriseSunsetAsync(date, geoData);
                processedSolarData = _processor.ProcessJsonForSolarData(solarData);
                processedSolarData.GeocodingDataId = geoData.Id;

                await _repository.AddSolarDataAsync(processedSolarData);
                await _repository.SaveChangesAsync();
            }

            return processedSolarData;
        }

        public async Task<bool> DeleteCity(int id)
        {
            _repository.DeleteCity(id);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSolarData(int id)
        {
            _repository.DeleteSolarData(id);
            await _repository.SaveChangesAsync();

            return true;
        }

    }
}
