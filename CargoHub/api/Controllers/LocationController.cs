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
}
