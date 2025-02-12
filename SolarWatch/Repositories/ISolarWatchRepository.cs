using SolarWatch.Models;

namespace SolarWatch.Repositories
{
    public interface ISolarWatchRepository
    {
        Task<GeocodingData?> GetGeocodingDataByNameAsync(string name);
        Task AddGeocodingDataAsync(GeocodingData geocodingData);
        Task<SolarData?> GetSolarDataAsync(DateOnly date, int geocodingDataId);
        Task AddSolarDataAsync(SolarData solarData);
        Task SaveChangesAsync();
        void DeleteCity(int id);
        void DeleteSolarData(int id);
    }
}
