using Microsoft.AspNetCore.Mvc;

[Route("api/v1/shipments")]
public class ShipmentController : Controller
{
    private readonly IShipmentService _shipmentService;
    private readonly IOrderService _orderService;
    public ShipmentController(IShipmentService shipmentService, IOrderService orderService)
    {
        _shipmentService = shipmentService;
        _orderService = orderService;
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

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetShipmentItems(int id){
        Shipment? shipment = _shipmentService.GetShipmentById(id);
        if (shipment is null) return BadRequest();
        ItemSmall[] items = _shipmentService.GetShipmentItems(shipment);
        return Ok(items);
    }

    [HttpGet("{id}/orders")]
    public async Task<IActionResult> GetOrderIdsRelatedToShipment(int id){
        int[] orderIds = _orderService.GetOrderIdsRelatedToShipment(id);
        return Ok(orderIds);
    }

    [HttpPost]
    public async Task<IActionResult> AddShipment([FromBody] Shipment shipment){
        bool result = await _shipmentService.AddShipment(shipment);
        return result ? Ok() : BadRequest("Shipment id already in use");
    }
}
