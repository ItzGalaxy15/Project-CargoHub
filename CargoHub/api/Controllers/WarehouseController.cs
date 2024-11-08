using Microsoft.AspNetCore.Mvc;

[Route("api/v1/warehouses")]
public class WarehouseController : Controller
{
    private readonly IWarehouseService _warehouseService;
    private readonly ILocationService _locationService;
    private readonly IWarehouseValidationService _warehouseValidationService;

    public WarehouseController(IWarehouseService warehouseService, ILocationService locationService,
     IWarehouseValidationService warehouseValidationService)
    {
        _warehouseService = warehouseService;
        _warehouseValidationService = warehouseValidationService;
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
        if (!_warehouseValidationService.IsWarehouseValid(warehouse)) return BadRequest("invalid warehouse object");
        await _warehouseService.AddWarehouse(warehouse);
        return CreatedAtAction(nameof(GetWarehouseById), new { id = warehouse.Id }, warehouse);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceWarehouse([FromBody] Warehouse warehouse, int id)
    {
        if (warehouse?.Id != id) return BadRequest("Invalid warehouse Id");
        if (!_warehouseValidationService.IsWarehouseValid(warehouse, true)) return BadRequest("invalid warehouse object");
        Warehouse? oldWarehouse = _warehouseService.GetWarehouseById(id);
        warehouse.CreatedAt = oldWarehouse!.CreatedAt;
        await _warehouseService.ReplaceWarehouse(warehouse, id);
        return Ok();
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
