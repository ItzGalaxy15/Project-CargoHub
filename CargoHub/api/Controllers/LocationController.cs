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
}
