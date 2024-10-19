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
}
