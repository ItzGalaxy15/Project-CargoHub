using Microsoft.AspNetCore.Mvc;

[Route("api/v1/warehouses")]
public class WarehouseController : Controller
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
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

}
