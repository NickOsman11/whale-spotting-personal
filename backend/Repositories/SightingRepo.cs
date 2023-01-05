using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhaleSpotting.Models.Database;
using WhaleSpotting.Models.Request;

namespace WhaleSpotting.Repositories
{
    public interface ISightingRepo
    {
        IEnumerable<Sighting> GetSightings(GetSightingsRequest request);
        IEnumerable<Sighting> GetSightingsBySpeciesId(int? speciesId);
        IEnumerable<Sighting> GetPendingSightings();
        Sighting CreateSighting(Sighting createSightingRequest);
        Sighting ConfirmRequest(int sightingId);
        Sighting RejectRequest(int sightingId);
        IEnumerable<Sighting> GetSightingsByLocationId(int? locationId);
        Sighting GetSightingById(int sightingId);
    }

    public class SightingRepo : ISightingRepo
    {
        private readonly WhaleSpottingDbContext _context;

        public SightingRepo(WhaleSpottingDbContext context)
        {
            _context = context;
        }

        public Sighting CreateSighting(Sighting newSighting)
        {
            var insertedSighting = _context.Sightings.Add(newSighting);
            _context.SaveChanges();

            return insertedSighting.Entity;
        }

        public IEnumerable<Sighting> GetSightings(GetSightingsRequest request)
        {
            return _context
                .Sightings
                .Include(s => s.Species)
                .Include(s => s.Location)
                .Where(s => (request.SpeciesId != null ? s.Species.Id == request.SpeciesId : true ))
                .Where(s => (request.LocationId != null ? s.Location.Id == request.LocationId : true ))
                .Where(s => s.ConfirmationStatus == ConfirmationStatus.Approved);
        }

        public IEnumerable<Sighting> GetPendingSightings()
        {
            return _context.Sightings.Where(s => s.ConfirmationStatus == ConfirmationStatus.Pending);
        }

        public IEnumerable<Sighting> GetSightingsBySpeciesId(int? speciesId)
        {
            return _context.Sightings
                .Include(s => s.Species)
                .Include(s => s.Location)
                .Where(s => s.Species.Id == speciesId)
                .Where(s => s.ConfirmationStatus == ConfirmationStatus.Approved)
                .OrderByDescending(s => s.SeenOn);
        }

        public Sighting ConfirmRequest(int sightingId)
        {
            var sighting = _context.Sightings.Single(s => s.Id == sightingId);
            if (sighting != null)
            {
                sighting.ConfirmationStatus = ConfirmationStatus.Approved;
                _context.SaveChanges();
                return sighting;
            }
            throw new ArgumentException($"The sighting with ID {sightingId} could not be found");
        }
        
        public Sighting RejectRequest(int sightingId)
        {
            var sighting = _context.Sightings.Single(s => s.Id == sightingId);
            if (sighting != null)
            {
                sighting.ConfirmationStatus = ConfirmationStatus.Rejected;
                _context.SaveChanges();
                return sighting;
            }
            throw new ArgumentException($"The sighting with ID {sightingId} could not be found");
        }

        public IEnumerable<Sighting> GetSightingsByLocationId(int? locationId)
        {
            return _context.Sightings
                .Include(s => s.Species)
                .Include(s => s.Location)
                .Where(s => s.Location.Id == locationId)
                .Where(s => s.ConfirmationStatus == ConfirmationStatus.Approved)
                .OrderByDescending(s => s.SeenOn);
        }
        
        public Sighting GetSightingById(int sightingId)
        {
            return _context.Sightings
                .Include(s => s.Species)
                .Include(s => s.Location)
                .Single(s => s.Id == sightingId);
        }
    }
}
