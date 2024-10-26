using Microsoft.AspNetCore.Mvc;

[Route("api/v1/locations")]
public class LocationController : Controller
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        Location[] locations = await _locationService.GetLocations();
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
        bool isValid = await _locationService.LocationIsValid(newLocation);
        if (!isValid) return BadRequest();
        await _locationService.AddLocation(newLocation);
        return StatusCode(201);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(int id, [FromBody] Location updatedLocation)
    {
        bool isUpdated = await _locationService.UpdateLocation(id, updatedLocation);
        if (!isUpdated) return BadRequest(); 
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
