using Microsoft.AspNetCore.Mvc;

[Route("api/v1/shipments")]
public class ShipmentController : Controller
{
    private readonly IShipmentService _shipmentService;
    public ShipmentController(IShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [HttpGet]
    public IActionResult GetShipments()
    {
        return Ok(_shipmentService.GetShipments());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShipmentById(int id)
    {
        var shipment = _shipmentService.GetShipmentById(id);
        return shipment is null ? BadRequest() : Ok(shipment);
    }
}
