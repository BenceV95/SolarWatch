using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Services
{
    public interface ISolarDataService
    {
        Task<GeocodingData> GetGeocodingDataAsync(string location);
        Task<SolarData> GetSolarDataAsync(DateOnly date, string location);
        Task<bool> DeleteCity([FromQuery, Required] int id);
        Task<bool> DeleteSolarData([FromQuery, Required] int id);
    }
}
