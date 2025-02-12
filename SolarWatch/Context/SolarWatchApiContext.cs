using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SolarWatch.Models;

namespace SolarWatch.Context
{
    public class SolarWatchApiContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<GeocodingData> GeocodingData { get; set; }
        public DbSet<SolarData> SolarData { get; set; }

        public SolarWatchApiContext (DbContextOptions<SolarWatchApiContext> options) : base(options)
        {
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Utils.ConnectionString);
        }
        */

    }
}
