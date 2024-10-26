using Microsoft.AspNetCore.Mvc;

[Route("api/v1/orders")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IOrderValidationService _orderValidationService;
    public OrderController(IOrderService orderService, IOrderValidationService orderValidationService){
        _orderService = orderService;
        _orderValidationService = orderValidationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(){
        return Ok(_orderService.GetOrders());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id){
        Order? order = _orderService.GetOrderById(id);
        return order is null ? BadRequest() : Ok(order);
    }

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetOrderItems(int id){
        Order? order = _orderService.GetOrderById(id);
        if (order is null) return BadRequest();
        ItemSmall[] items = _orderService.GetOrderItems(order);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] Order order){
        await _orderService.AddOrder(order);
        if (!_orderValidationService.IsOrderValid(order)) return BadRequest("Invalid order");

        return Created();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id){
        Order? order = _orderService.GetOrderById(id);
        if (order is null) return BadRequest("Order not found");
        await _orderService.DeleteOrder(order);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceOrder([FromBody] Order order, int id){
        if (order?.Id != id) return BadRequest("Invalid id");
        if (!_orderValidationService.IsOrderValid(order, true)) return BadRequest("Invalid order");

        await _orderService.ReplaceOrder(order, id);
        return Ok();
    }

    [HttpPut("{id}/items")]
    public async Task<IActionResult> Items(int id){
        // Is broken / confusing in Python version.
        return StatusCode(501);
    }
}
