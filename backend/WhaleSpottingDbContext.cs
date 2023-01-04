using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WhaleSpotting.Models.Database;

namespace WhaleSpotting
{
    public class WhaleSpottingDbContext : DbContext
    {
        public DbSet<ConservationStatus> ConservationStatuses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Sighting> Sightings { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder
        )
        {
            optionsBuilder.UseNpgsql(GetConnectionString());
        }

        private static string GetConnectionString()
        {
            var databaseUrl =
                Environment.GetEnvironmentVariable("DATABASE_URL");
            if (databaseUrl == null)
            {
                throw new Exception("Environment variable 'DATABASE_URL' must not be null");
            }

            bool useSsl = true;
            var useSslVariable = Environment.GetEnvironmentVariable("USE_SSL");
            if (useSslVariable != null)
            {
                if (!Boolean.TryParse(useSslVariable, out useSsl))
                {
                    throw new Exception("Environment variable 'USE_SSL' must be parse-able as bool");
                }
            }

            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder =
                new NpgsqlConnectionStringBuilder {
                    Host = databaseUri.Host,
                    Port = databaseUri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = databaseUri.LocalPath.TrimStart('/')
                };
            if (useSsl)
            {
                builder.SslMode = SslMode.Require;
                builder.TrustServerCertificate = true;
            }

            return builder.ToString();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>().HasData(new Location { Id = 1, Description = "Norwegian Sea" });
            modelBuilder.Entity<Species>().HasData(new Species { Id = 1, Name = "Humpback Whale", ScientificName = "Megaptera novaeangliae", Description = "The humpback whale is a species of baleen whale. It is a rorqual and is the only species in the genus Megaptera. Adults range in length from 14–17 m and weigh up to 40 metric tons. The humpback has a distinctive body shape, with long pectoral fins and tubercles on its head.", PhotoUrl = "https://images.immediate.co.uk/production/volatile/sites/23/2019/10/GettyImages-1164887104_Craig-Lambert-2faf563.jpg?quality=90&webp=true&resize=1000,667" });
            modelBuilder.Entity<Species>().HasData(new Species { Id = 2, Name = "Blue Whale", ScientificName = "Balaenoptera musculus", Description = "The blue whale is a marine mammal and a baleen whale. Reaching a maximum confirmed length of 29.9 meters and weighing up to 199 tonnes, it is the largest animal known to have ever existed. The blue whale's long and slender body can be of various shades of greyish-blue dorsally and somewhat lighter underneath.", PhotoUrl = "https://newsroom.unsw.edu.au/sites/default/files/styles/full_width__2x/public/thumbnails/image/pygmy_blue_whale_swimming_underwater_3.jpg?itok=NxFYiUIS" });
            modelBuilder.Entity<Sighting>().HasData(new { Id = 1, SpeciesId = 1, SeenBy = "Johnny Fakeson", SeenOn= new DateOnly(2023,01,02), ImageUrl = "https://i.guim.co.uk/img/media/9b91662f6485bbc457740c71777f5156ae9f7511/0_55_3500_2101/master/3500.jpg?width=620&quality=45&dpr=2&s=none", Description =  "Wow!", WhaleCount = 2, ConfirmationStatus = ConfirmationStatus.Approved, LocationId = 1, Latitude = 64.35f, Longitude = 3.38f});
        }
    }
}
