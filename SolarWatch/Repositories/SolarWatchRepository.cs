using Microsoft.EntityFrameworkCore;
using SolarWatch.Context;
using SolarWatch.Models;

namespace SolarWatch.Repositories
{
    public class SolarWatchRepository : ISolarWatchRepository
    {
        private readonly SolarWatchApiContext _context;

        public SolarWatchRepository(SolarWatchApiContext context)
        {
            _context = context;
        }

        public async Task<GeocodingData?> GetGeocodingDataByNameAsync(string name)
        {
            return await _context.GeocodingData.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task AddGeocodingDataAsync(GeocodingData geocodingData)
        {
            await _context.GeocodingData.AddAsync(geocodingData);
        }

        public async Task<SolarData?> GetSolarDataAsync(DateOnly date, int geocodingDataId)
        {
            return await _context.SolarData.FirstOrDefaultAsync(x => x.Date == date && x.GeocodingDataId == geocodingDataId);
        }

        public async Task AddSolarDataAsync(SolarData solarData)
        {
            await _context.SolarData.AddAsync(solarData);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void DeleteCity(int id)
        {
            _context.GeocodingData.RemoveRange(_context.GeocodingData.Where(x => x.Id == id));
        }

        public void DeleteSolarData(int id)
        {
            _context.SolarData.RemoveRange(_context.SolarData.Where(x => x.Id == id));
        }
    }
}
