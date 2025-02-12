using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SolarWatch.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeocodingData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeocodingData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolarData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sunrise = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Sunset = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SolarNoon = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DayLength = table.Column<int>(type: "integer", nullable: false),
                    CivilTwilightBegin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CivilTwilightEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NauticalTwilightBegin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NauticalTwilightEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AstronomicalTwilightBegin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AstronomicalTwilightEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeZoneID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeocodingData");

            migrationBuilder.DropTable(
                name: "SolarData");
        }
    }
}
