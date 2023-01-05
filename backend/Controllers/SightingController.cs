using Microsoft.AspNetCore.Mvc;
using WhaleSpotting.Services;
using WhaleSpotting.Models.Response;
using WhaleSpotting.Models.Database;
using WhaleSpotting.Models.Request;
using WhaleSpotting.Helpers;
using System;
using System.Threading.Tasks;

namespace WhaleSpotting.Controllers
{
    [ApiController]
    [Route("/sightings")]
    public class SightingController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISightingService _sightings;

        public SightingController
        (
            IAuthService authService,
            ISightingService sightings
        )
        {
            _authService = authService;
            _sightings = sightings;
        }

        [HttpGet]
        public ActionResult<ListResponse<Sighting>> GetSightings([FromQuery] GetSightingsRequest request)
        {
            try
            {
                var sightings = _sightings.GetSightings(request);
                return new ListResponse<Sighting>(sightings);
            }
            catch
            {
                //TODO: proper error handling
                return BadRequest();
            }
        }

        [HttpGet("species/{speciesId}")]
        public ActionResult<ListResponse<Sighting>> GetSightingsBySpeciesId([FromRoute] int speciesId)
        {
            var sightings = _sightings.GetSightingsBySpeciesId(speciesId);
            return new ListResponse<Sighting>(sightings);
        }

        [HttpGet("pending")]
        public ActionResult<ListResponse<Sighting>> GetPendingSightings()
        {
            var pendingSightings = _sightings.GetPendingSightings();
            return new ListResponse<Sighting>(pendingSightings);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSightingAsync([FromBody] CreateSightingRequest createSightingRequest)
        {
            var createdSighting = await _sightings.CreateSightingAsync(createSightingRequest);
            return Created("/api", createdSighting);
        }

        [HttpGet("locations/{locationId}")]
        public ActionResult<ListResponse<Sighting>> GetSightingsByLocationId([FromRoute] int locationId)
        {
            var sightings = _sightings.GetSightingsByLocationId(locationId);
            return new ListResponse<Sighting>(sightings);
        }

        [HttpPatch("{sightingId}/confirmation")]
        public ActionResult ChangeConfirmationStatus(
            [FromHeader] string authorization, 
            [FromRoute] int sightingId,
            [FromBody] ConfirmOrRejectRequest confirmOrRejectRequest)
        {
            if (authorization is null)
            {
                return new UnauthorizedResult();
            }
            try
            {
                (string username, string password) = AuthHelper.GetUsernameAndPassword(authorization);

                var check = _authService.IsValidLoginInfo(username, password);
                if (!check)
                {
                    return Unauthorized();
                }

                var result = _sightings.ConfirmOrRejectSighting(confirmOrRejectRequest, sightingId);
                if (result != null)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        
        [HttpGet("{sightingId}")]
        public ActionResult<Sighting> GetSightingById([FromRoute] int sightingId)
        {
            try
            {
                var sighting = _sightings.GetSightingById(sightingId);
                return sighting;
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
