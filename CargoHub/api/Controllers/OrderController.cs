using Microsoft.AspNetCore.Mvc;

[Route("api/v1/orders")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService){
        _orderService = orderService;
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
        bool result = await _orderService.AddOrder(order);
        return result ? Created() : BadRequest("Order id already in use");
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
        bool result = await _orderService.ReplaceOrder(order, id);
        return result ? Ok() : BadRequest("Order not found");
    }

    [HttpPut("{id}/items")]
    public async Task<IActionResult> Items(int id){
        // Is broken / confusing in Python version.
        return StatusCode(501);
    }
}
