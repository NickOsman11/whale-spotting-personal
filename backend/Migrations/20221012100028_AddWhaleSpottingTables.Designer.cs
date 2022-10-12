﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WhaleSpotting;

#nullable disable

namespace WhaleSpotting.Migrations
{
    [DbContext(typeof(WhaleSpottingDbContext))]
    [Migration("20221012100028_AddWhaleSpottingTables")]
    partial class AddWhaleSpottingTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LocationSpecies", b =>
                {
                    b.Property<int>("LocationsId")
                        .HasColumnType("integer");

                    b.Property<int>("SpeciesId")
                        .HasColumnType("integer");

                    b.HasKey("LocationsId", "SpeciesId");

                    b.HasIndex("SpeciesId");

                    b.ToTable("LocationSpecies");
                });

            modelBuilder.Entity("WhaleSpotting.Models.Database.ConfirmationStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ConfirmationStatuses");
                });

            modelBuilder.Entity("WhaleSpotting.Models.Database.ConservationStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ConservationStatuses");
                });

            modelBuilder.Entity("WhaleSpotting.Models.Database.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("WhaleSpotting.Models.Database.Sighting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ConfirmationStatusId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<int?>("LocationId")
                        .HasColumnType("integer");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<string>("SeenBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("SeenOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("SpeciesId")
                        .HasColumnType("integer");

                    b.Property<int>("WhaleCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ConfirmationStatusId");

                    b.HasIndex("LocationId");

                    b.HasIndex("SpeciesId");

                    b.ToTable("Sightings");
                });

            modelBuilder.Entity("WhaleSpotting.Models.Database.Species", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ConservationStatusId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("ScientificName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ConservationStatusId");

                    b.ToTable("Species");
                });

            modelBuilder.Entity("WhaleSpotting.Models.Database.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("HashedPassword")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("bytea");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LocationSpecies", b =>
                {
                    b.HasOne("WhaleSpotting.Models.Database.Location", null)
                        .WithMany()
                        .HasForeignKey("LocationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WhaleSpotting.Models.Database.Species", null)
                        .WithMany()
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WhaleSpotting.Models.Database.Sighting", b =>
                {
                    b.HasOne("WhaleSpotting.Models.Database.ConfirmationStatus", "ConfirmationStatus")
                        .WithMany()
                        .HasForeignKey("ConfirmationStatusId");

                    b.HasOne("WhaleSpotting.Models.Database.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("WhaleSpotting.Models.Database.Species", "Species")
                        .WithMany()
                        .HasForeignKey("SpeciesId");

                    b.Navigation("ConfirmationStatus");

                    b.Navigation("Location");

                    b.Navigation("Species");
                });

            modelBuilder.Entity("WhaleSpotting.Models.Database.Species", b =>
                {
                    b.HasOne("WhaleSpotting.Models.Database.ConservationStatus", "ConservationStatus")
                        .WithMany()
                        .HasForeignKey("ConservationStatusId");

                    b.Navigation("ConservationStatus");
                });
#pragma warning restore 612, 618
        }
    }
}
