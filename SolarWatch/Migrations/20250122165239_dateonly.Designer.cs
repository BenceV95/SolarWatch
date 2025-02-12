﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SolarWatch.Context;

#nullable disable

namespace SolarWatch.Migrations
{
    [DbContext(typeof(SolarWatchApiContext))]
    [Migration("20250122165239_dateonly")]
    partial class dateonly
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SolarWatch.Models.GeocodingData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GeocodingData");
                });

            modelBuilder.Entity("SolarWatch.Models.SolarData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AstronomicalTwilightBegin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("AstronomicalTwilightEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CivilTwilightBegin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CivilTwilightEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("DayLength")
                        .HasColumnType("integer");

                    b.Property<int>("GeocodingDataId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("NauticalTwilightBegin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("NauticalTwilightEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("SolarNoon")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Sunrise")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Sunset")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TimeZoneID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GeocodingDataId");

                    b.ToTable("SolarData");
                });

            modelBuilder.Entity("SolarWatch.Models.SolarData", b =>
                {
                    b.HasOne("SolarWatch.Models.GeocodingData", null)
                        .WithMany("SolarDatas")
                        .HasForeignKey("GeocodingDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SolarWatch.Models.GeocodingData", b =>
                {
                    b.Navigation("SolarDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
