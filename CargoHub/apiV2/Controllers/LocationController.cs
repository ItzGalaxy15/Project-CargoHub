using System.IO;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace apiV2.Controllers
{
    [Route("api/v2/locations")]
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
        public IActionResult GetLocations()
        {
            var response = this.HttpContext.Items["FilteredLocations"] as IEnumerable<object>;

            if (response == null)
            {
                return this.StatusCode(500, "Server error: no data available.");
            }

            return this.Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            Location? location = await this.locationService.GetLocationById(id);
            if (location == null)
            {
                return this.NotFound($"Location with ID {id} not found.")
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
                return this.NotFound();
            }

            await this.locationService.DeleteLocation(location);
            return this.Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchLocation(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            bool isValid = await this.locationValidationService.IsLocationValidForPATCH(patch, id);
            if (!isValid)
            {
                return this.BadRequest();
            }

            Location? location = await this.locationService.GetLocationById(id);
            await this.locationService.PatchLocation(id, patch, location!);
            return this.Ok();
        }
    }
}