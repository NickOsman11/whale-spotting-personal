using System.Collections.Generic;
using WhaleSpotting.Repositories;
using WhaleSpotting.Models.Api;
using WhaleSpotting.Models.Database;
using WhaleSpotting.Models.Request;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WhaleSpotting.Services
{
    public interface ISightingService
    {
        IEnumerable<Sighting> GetSightings(GetSightingsRequest request);
        IEnumerable<Sighting> GetPendingSightings();
        IEnumerable<Sighting> GetSightingsBySpeciesId(int speciesId);
        Task<Sighting> CreateSightingAsync(CreateSightingRequest request);
        IEnumerable<Sighting> GetSightingsByLocationId(int locationId);
        Sighting ConfirmOrRejectSighting(ConfirmOrRejectRequest confirmOrRejectSighting, int sightingId);
        Sighting GetSightingById(int sightingId);
        Task<Location> GetLocationByCoordinatesAsync(double latitude, double longitude);
    }

    public class SightingService : ISightingService
    {
        private readonly ISightingRepo _sightings;
        private readonly IWhaleRepo _whales;
        private readonly ILocationRepo _locations;

        public SightingService(ISightingRepo sightings, IWhaleRepo whales, ILocationRepo locations)
        {
            _sightings = sightings;
            _whales = whales;
            _locations = locations;
        }

        public IEnumerable<Sighting> GetSightings(GetSightingsRequest request)
        {
            return _sightings.GetSightings(request);
        }

        public IEnumerable<Sighting> GetSightingsBySpeciesId(int speciesId)
        {
            return _sightings.GetSightingsBySpeciesId(speciesId);
        }

        public IEnumerable<Sighting> GetPendingSightings()
        {
            return _sightings.GetPendingSightings();
        }

        public async Task<Sighting> CreateSightingAsync(CreateSightingRequest request)
        {
            var getLocation = await GetLocationByCoordinatesAsync(request.Latitude, request.Longitude);
            var newSighting = new Sighting
            {
                SeenBy = request.SeenBy,
                SeenOn = request.SeenOn,
                ImageUrl = request.ImageUrl,
                Species = _whales.GetSpeciesById(request.SpeciesId),
                Description = request.Description,
                Location = getLocation,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                WhaleCount = request.WhaleCount,
                ConfirmationStatus = ConfirmationStatus.Pending,
            };

            return _sightings.CreateSighting(newSighting);
        }

        public Sighting ConfirmOrRejectSighting(ConfirmOrRejectRequest confirmOrRejectSighting, int sightingId)
        {
            if (confirmOrRejectSighting.NewConfirmationStatus == ConfirmationStatus.Approved)
            {
                return _sightings.ConfirmRequest(sightingId);
            }
            else if (confirmOrRejectSighting.NewConfirmationStatus == ConfirmationStatus.Rejected)
            {
                return _sightings.RejectRequest(sightingId);
            }
            throw new ArgumentOutOfRangeException("The confirmation request was not to approve or reject the sighting. " +
                $"To approve, the NewConfirmationStatus of the request should be {(int)ConfirmationStatus.Approved}." +
                $"To reject, it should be {(int)ConfirmationStatus.Rejected}.");
        }

        public IEnumerable<Sighting> GetSightingsByLocationId(int locationId)
        {
            return _sightings.GetSightingsByLocationId(locationId);
        }

        public Sighting GetSightingById(int sightingId)
        {
            return _sightings.GetSightingById(sightingId);
        }

        public async Task<Location> GetLocationByCoordinatesAsync(double latitude, double longitude)
        {
            var accessKey = Environment.GetEnvironmentVariable("POSITION_STACK_KEY");
            string apiUrl = $"http://api.positionstack.com/v1/reverse?access_key={accessKey}&query={latitude},{longitude}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(apiUrl))
                    {
                        using (HttpContent content = res.Content)
                        {
                            var data = JsonConvert.DeserializeObject<PositionStackResponse>(await content.ReadAsStringAsync());
                            if (data != null)
                            {
                                var longFormName = data.Data.First().Country ?? data.Data.First().Name;
                                var location = _locations.GetOrCreateLocationByName(longFormName);
                                return location;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }
        }
    }
}
