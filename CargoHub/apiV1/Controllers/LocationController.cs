using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/locations")]
    public class LocationController : Controller
    {
        private readonly ILocationService locationService;

        private readonly ILocationValidationService locationValidationService;

        public LocationController(ILocationService locationService, ILocationValidationService locationValidationService)
        {
            this.locationService = locationService;
            this.locationValidationService = locationValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            Location[] locations = await this.locationService.GetLocations();
            return this.Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            Location? location = await this.locationService.GetLocationById(id);
            if (location == null)
            {
                this.NotFound($"Location with ID {id} not found.");
            }

            return this.Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation([FromBody] Location newLocation)
        {
            bool isValid = await this.locationValidationService.IsLocationValidForPOST(newLocation);
            if (!isValid)
            {
                return this.BadRequest();
            }

            await this.locationService.AddLocation(newLocation);
            return this.CreatedAtAction(nameof(this.GetLocationById), new { id = newLocation.Id }, newLocation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] Location updatedLocation)
        {
            bool isValid = await this.locationValidationService.IsLocationValidForPUT(updatedLocation, id);
            if (!isValid)
            {
                return this.BadRequest();
            }

            Location? oldLocation = await this.locationService.GetLocationById(id);
            updatedLocation.CreatedAt = oldLocation!.CreatedAt;
            await this.locationService.UpdateLocation(id, updatedLocation);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            Location? location = await this.locationService.GetLocationById(id);
            if (location == null)
            {
                return this.NotFound($"Location with ID {id} not found.");
            }

            await this.locationService.DeleteLocation(location);
            return this.Ok();
        }
    }
}
