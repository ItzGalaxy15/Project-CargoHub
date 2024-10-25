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
    public async Task<IActionResult> GetWarehouseById(int id)
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
        return result ?  CreatedAtAction(nameof(GetWarehouseById), new { id = warehouse.Id }, warehouse)
                        : BadRequest("warehouse id already in use");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceWarehouse([FromBody] Warehouse warehouse, int id)
    {
        bool result = await _warehouseService.ReplaceWarehouse(warehouse, id);
        return result ? Ok() : BadRequest("warehouse not found");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWarehouse(int id)
    {
        Warehouse? warehouse = _warehouseService.GetWarehouseById(id);
        if (warehouse is null) return BadRequest();
        await _warehouseService.DeleteWarehouse(warehouse);
        return Ok();
    }
}
