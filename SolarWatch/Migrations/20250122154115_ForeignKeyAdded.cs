using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarWatch.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "SolarData",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "GeocodingDataId",
                table: "SolarData",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SolarData_GeocodingDataId",
                table: "SolarData",
                column: "GeocodingDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_SolarData_GeocodingData_GeocodingDataId",
                table: "SolarData",
                column: "GeocodingDataId",
                principalTable: "GeocodingData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolarData_GeocodingData_GeocodingDataId",
                table: "SolarData");

            migrationBuilder.DropIndex(
                name: "IX_SolarData_GeocodingDataId",
                table: "SolarData");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "SolarData");

            migrationBuilder.DropColumn(
                name: "GeocodingDataId",
                table: "SolarData");
        }
    }
}
