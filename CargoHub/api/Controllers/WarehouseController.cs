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
    public IActionResult GetWarehouses()
    {
        return Ok(_warehouseService.GetWarehouses());
    }

}
