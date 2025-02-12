namespace SolarWatch.Models
{
    // make sure &formatted=0 is in the url !!!
    public class SolarData
    {
        public int Id { get; set; }
        // Foreign Key
        public int GeocodingDataId { get; set; }
        public DateOnly Date { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public DateTime SolarNoon { get; set; }
        public int DayLength { get; set; }
        public DateTime CivilTwilightBegin { get; set; }
        public DateTime CivilTwilightEnd { get; set; }
        public DateTime NauticalTwilightBegin { get; set; }
        public DateTime NauticalTwilightEnd { get; set; }
        public DateTime AstronomicalTwilightBegin { get; set; }
        public DateTime AstronomicalTwilightEnd { get; set; }

        public string TimeZoneID { get; set; }

        public TimeSpan DayLengthTimeSpan => TimeSpan.FromSeconds(DayLength);

        public string DayLengthFormatted => DayLengthTimeSpan.ToString(@"hh\:mm\:ss");

        public void ConvertToUtc()
        {
            Sunrise = Sunrise.ToUniversalTime();
            Sunset = Sunset.ToUniversalTime();
            SolarNoon = SolarNoon.ToUniversalTime();
            CivilTwilightBegin = CivilTwilightBegin.ToUniversalTime();
            CivilTwilightEnd = CivilTwilightEnd.ToUniversalTime();
            NauticalTwilightBegin = NauticalTwilightBegin.ToUniversalTime();
            NauticalTwilightEnd = NauticalTwilightEnd.ToUniversalTime();
            AstronomicalTwilightBegin = AstronomicalTwilightBegin.ToUniversalTime();
            AstronomicalTwilightEnd = AstronomicalTwilightEnd.ToUniversalTime();
        }
    }
}
