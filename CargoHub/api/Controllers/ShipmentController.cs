using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/shipments")]
public class ShipmentController : Controller
{
    private readonly IShipmentService _shipmentService;
    private readonly IShipmentValidationService _shipmentValidationService;
    private readonly IOrderService _orderService;
    public ShipmentController(IShipmentService shipmentService, IShipmentValidationService shipmentValidationService, IOrderService orderService)
    {
        _shipmentService = shipmentService;
        _shipmentValidationService = shipmentValidationService;
        _orderService = orderService;
    }

    // Returns all shipments
    [HttpGet]
    public IActionResult GetShipments()
    {
        return Ok(_shipmentService.GetShipments());
    }

    // Returns a shipment by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShipmentById(int id)
    {
        var shipment = await Task.Run(() =>  _shipmentService.GetShipmentById(id));
        return shipment is null ? BadRequest() : Ok(shipment);
    }

    // Returns all items in a shipment
    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetShipmentItems(int id){
        Shipment? shipment = await Task.Run(() => _shipmentService.GetShipmentById(id));
        if (shipment is null) return BadRequest();
        ItemSmall[] items = _shipmentService.GetShipmentItems(shipment);
        return Ok(items);
    }

    // Returns all orders related to a shipment
    [HttpGet("{id}/orders")]
    public async Task<IActionResult> GetOrderIdsRelatedToShipment(int id){
        int[] orderIds = await Task.Run(() => _orderService.GetOrderIdsRelatedToShipment(id));
        return Ok(orderIds);
    }

    // Adds a new shipment
    [HttpPost]
    public async Task<IActionResult> AddShipment([FromBody] Shipment shipment){
        if (!_shipmentValidationService.IsShipmentValid(shipment)) return BadRequest("Invalid shipment object");
        await _shipmentService.AddShipment(shipment);
        return CreatedAtAction(nameof(GetShipmentById), new { id = shipment.Id }, shipment);
    }

    // Replaces a shipment with a new one
    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceShipment([FromBody] Shipment shipment, int id){
        if (shipment?.Id != id) return BadRequest("Invalid id");
        if (!_shipmentValidationService.IsShipmentValid(shipment, true)) return BadRequest("Invalid shipment object");
        Shipment? oldShipment = _shipmentService.GetShipmentById(id);
        shipment.CreatedAt = oldShipment!.CreatedAt;
        await _shipmentService.ReplaceShipment(shipment, id);
        return Ok();
    }


    // Should probably become a PATCH request in v2
    [HttpPut("{id}/orders")]
    public async Task<IActionResult> UpdateOrdersInShipment(int id, [FromBody] int[] orderIds){
        // Maybe check if shipment exists?
        bool result = await _orderService.UpdateOrdersWithShipmentId(id, orderIds);
        return result ? Ok() : BadRequest("Invalid provided order id's"); // false not implemented yet
    }


    // [HttpPut("{id}/items")]
    // public async Task<IActionResult> Items(int id){
    //     // Is broken / confusing in Python version.
    //     return StatusCode(501);
    // }

    // [HttpPut("{id}/commit")]
    // public async Task<IActionResult> Commit(int id){
    //     // Is broken / confusing in Python version.
    //     return StatusCode(501);
    // }    
    
    // Deletes a shipment
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShipment(int id){
        Shipment? shipment = _shipmentService.GetShipmentById(id);
        if (shipment is null) return BadRequest();
        await _shipmentService.DeleteShipment(shipment);
        return Ok();
    }
}
