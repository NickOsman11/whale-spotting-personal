using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WhaleSpotting.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "SeenOn",
                table: "Sightings",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Description" },
                values: new object[] { 1, "Norwegian Sea" });

            migrationBuilder.InsertData(
                table: "Species",
                columns: new[] { "Id", "ConservationStatusId", "Description", "Name", "PhotoUrl", "ScientificName" },
                values: new object[,]
                {
                    { 1, null, "The humpback whale is a species of baleen whale. It is a rorqual and is the only species in the genus Megaptera. Adults range in length from 14–17 m and weigh up to 40 metric tons. The humpback has a distinctive body shape, with long pectoral fins and tubercles on its head.", "Humpback Whale", "https://images.immediate.co.uk/production/volatile/sites/23/2019/10/GettyImages-1164887104_Craig-Lambert-2faf563.jpg?quality=90&webp=true&resize=1000,667", "Megaptera novaeangliae" },
                    { 2, null, "The blue whale is a marine mammal and a baleen whale. Reaching a maximum confirmed length of 29.9 meters and weighing up to 199 tonnes, it is the largest animal known to have ever existed. The blue whale's long and slender body can be of various shades of greyish-blue dorsally and somewhat lighter underneath.", "Blue Whale", "https://newsroom.unsw.edu.au/sites/default/files/styles/full_width__2x/public/thumbnails/image/pygmy_blue_whale_swimming_underwater_3.jpg?itok=NxFYiUIS", "Balaenoptera musculus" }
                });

            migrationBuilder.InsertData(
                table: "Sightings",
                columns: new[] { "Id", "ConfirmationStatus", "Description", "ImageUrl", "Latitude", "LocationId", "Longitude", "SeenBy", "SeenOn", "SpeciesId", "WhaleCount" },
                values: new object[] { 1, 2, "Wow!", "https://i.guim.co.uk/img/media/9b91662f6485bbc457740c71777f5156ae9f7511/0_55_3500_2101/master/3500.jpg?width=620&quality=45&dpr=2&s=none", 64.35f, 1, 3.38f, "Johnny Fakeson", new DateOnly(2023, 1, 2), 1, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sightings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SeenOn",
                table: "Sightings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
