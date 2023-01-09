using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WhaleSpotting.Models.Database;
using WhaleSpotting.Repositories;

namespace WhaleSpotting.Services
{
    public interface ILocationService
    {
        IEnumerable<Location> GetAllLocations();
        Location GetLocationById(int locationId);
    }

    public class LocationService : ILocationService
    {
        private readonly ILocationRepo _locations;

        public LocationService(ILocationRepo locations)
        {
            _locations = locations;
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return _locations.GetAllLocations();
        }

        public Location GetLocationById(int locationId)
        {
            return _locations.GetLocationById(locationId);
        }
    }
}
