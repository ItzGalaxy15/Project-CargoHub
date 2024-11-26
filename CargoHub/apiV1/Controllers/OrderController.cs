using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/orders")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderValidationService _orderValidationService;
        public OrderController(IOrderService orderService, IOrderValidationService orderValidationService){
            _orderService = orderService;
            _orderValidationService = orderValidationService;
        }

        // Returns all orders
        [HttpGet]
        public async Task<IActionResult> GetOrders(){
            var orders = await Task.Run(() => _orderService.GetOrders());
            return Ok(orders);
        }

        // Returns an order by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id){
            Order? order = await Task.Run(() => _orderService.GetOrderById(id));
            return order is null ? NotFound() : Ok(order);
        }

        // Returns all items in an order
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetOrderItems(int id){
            Order? order = await Task.Run(() => _orderService.GetOrderById(id));
            if (order is null) return NotFound();
            ItemSmall[] items = _orderService.GetOrderItems(order);
            return Ok(items);
        }

        // Adds a new order
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Order order){
            
            if (!_orderValidationService.IsOrderValid(order)) return BadRequest("Invalid order");
            await _orderService.AddOrder(order);
            return Created();
        }

        // Replaces an order with a new one
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceOrder([FromBody] Order order, int id){
            if (order?.Id != id) return BadRequest("Invalid id");
            if (!_orderValidationService.IsOrderValid(order, true)) return BadRequest("Invalid order");

            Order? old_order = _orderService.GetOrderById(id);
            order.CreatedAt = old_order!.CreatedAt;

            await _orderService.ReplaceOrder(order, id);
            return Ok();
        }

        // Returns all items in an order
        // change to async when code is implemented
        [HttpPut("{id}/items")]
        public IActionResult Items(int id){
            // Is broken / confusing in Python version.
            return StatusCode(501);
        }

        // Deletes an order
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id){
            Order? order = _orderService.GetOrderById(id);
            if (order is null) return NotFound("Order not found");
            await _orderService.DeleteOrder(order);
            return Ok();
        }
    }
}
