using Microsoft.AspNetCore.Mvc;

[Route("api/v1/warehouses")]
public class WarehouseController : Controller
{
    private readonly IWarehouseService _warehouseService;
    private readonly ILocationService _locationService;

    public WarehouseController(IWarehouseService warehouseService, ILocationService locationService)
    {
        _warehouseService = warehouseService;
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWarehouses()
    {
        return Ok(_warehouseService.GetWarehouses());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWarehousetById(int id)
    {
        var warehouse = _warehouseService.GetWarehouseById(id);
        return warehouse is null ? BadRequest() : Ok(warehouse);
    }

    
    [HttpGet("{id}/locations")]
    public async Task<IActionResult> GetLocationsInWarehouse(int id) // id = warehouseId
    {
        Location[] locations = _locationService.GetLocationsInWarehouse(id);
        return Ok(locations);
    }

    [HttpPost]
    public async Task<IActionResult> AddWarehouse([FromBody] Warehouse warehouse)
    {
        bool result = await _warehouseService.AddWarehouse(warehouse);
        return result ?  CreatedAtAction(nameof(GetWarehousetById), new { id = warehouse.Id }, warehouse)
                        : BadRequest("warehouse id already in use");
    }


}
