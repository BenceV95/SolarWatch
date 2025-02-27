﻿namespace SolarWatch.Models
{
    public class GeocodingData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Country { get; set; }
        public string? State { get; set; }
        public ICollection<SolarData>? SolarDatas { get; set; }

    }
}
