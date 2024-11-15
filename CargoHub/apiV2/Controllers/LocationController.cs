using Microsoft.AspNetCore.Mvc;
using apiV1.ValidationInterfaces;
using apiV2.Interfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/locations")]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        private readonly ILocationValidationService _locationValidationService;

        public LocationController(ILocationService locationService, ILocationValidationService locationValidationService)
        {
            _locationService = locationService;
            _locationValidationService = locationValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            Location[] locations = await _locationService.GetLocations();
            Console.WriteLine("Hello V2");
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            Location? location = await _locationService.GetLocationById(id);
            if (location == null) return NotFound();
            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation([FromBody] Location newLocation)
        {
            bool isValid = await _locationValidationService.IsLocationValidForPOST(newLocation);
            if (!isValid) return BadRequest();
            await _locationService.AddLocation(newLocation);
            return CreatedAtAction(nameof(GetLocationById), new { id = newLocation.Id }, newLocation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] Location updatedLocation)
        {
            bool isValid = await _locationValidationService.IsLocationValidForPUT(updatedLocation, id);
            if (!isValid) return BadRequest(); 
            Location? oldLocation = await _locationService.GetLocationById(id);
            updatedLocation.CreatedAt = oldLocation!.CreatedAt;
            await _locationService.UpdateLocation(id, updatedLocation);
            return Ok(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            Location? location = await _locationService.GetLocationById(id);
            if (location == null) return BadRequest();
            await _locationService.DeleteLocation(location);
            return Ok();
        }
    }
}