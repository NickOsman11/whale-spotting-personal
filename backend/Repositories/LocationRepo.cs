using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WhaleSpotting.Models.Database;

namespace WhaleSpotting.Repositories
{
    public interface ILocationRepo
    {
        IEnumerable<Location> GetAllLocations();
        Location GetLocationById(int locationId);
        Location GetOrCreateLocationByName(string locationName); 
        Location AddLocation(Location newLocation);
    }

    public class LocationRepo : ILocationRepo
    {
        private readonly WhaleSpottingDbContext _context;

        public LocationRepo(WhaleSpottingDbContext context)
        {
            _context = context;
        }

        public Location GetLocationById(int locationId)
        {
            return _context.Locations
                .Include(l => l.Species)
                .Single(l => l.Id == locationId);
        }

        public Location GetOrCreateLocationByName(string locationName) 
        {
            var location = _context.Locations
                .SingleOrDefault(l => l.Description == locationName);
            
            return location ?? AddLocation(new Location { Description = locationName }); 
        }

        public Location AddLocation(Location newLocation)
        {
           var insertedLocation = _context.Locations.Add(newLocation);
           _context.SaveChanges();

           return insertedLocation.Entity;
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return _context.Locations.ToList();
        }
    }
}
