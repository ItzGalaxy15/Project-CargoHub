using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/orders")]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IOrderValidationService orderValidationService;

        public OrderController(IOrderService orderService, IOrderValidationService orderValidationService)
        {
            this.orderService = orderService;
            this.orderValidationService = orderValidationService;
        }

        // Returns all orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await Task.Run(() => this.orderService.GetOrders());
            return this.Ok(orders);
        }

        // Returns an order by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            Order? order = await Task.Run(() => this.orderService.GetOrderById(id));
            return order is null ? this.NotFound($"Order with ID {id} not found.") : this.Ok(order);
        }

        // Returns all items in an order
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetOrderItems(int id)
        {
            Order? order = await Task.Run(() => this.orderService.GetOrderById(id));
            if (order is null)
            {
                return this.NotFound();
            }

            ItemSmall[] items = this.orderService.GetOrderItems(order);
            return this.Ok(items);
        }

        // Adds a new order
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Order order)
        {
            if (!this.orderValidationService.IsOrderValid(order))
            {
                return this.BadRequest("Invalid order");
            }

            await this.orderService.AddOrder(order);
            return this.CreatedAtAction(nameof(this.GetOrderById), new { id = order.Id }, order);
        }

        // Replaces an order with a new one
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceOrder([FromBody] Order order, int id)
        {
            if (order?.Id != id)
            {
                return this.BadRequest("Invalid id");
            }

            if (!this.orderValidationService.IsOrderValid(order, true))
            {
                return this.BadRequest("Invalid order");
            }

            Order? old_order = this.orderService.GetOrderById(id);
            order.CreatedAt = old_order!.CreatedAt;

            await this.orderService.ReplaceOrder(order, id);
            return this.Ok();
        }

        // Returns all items in an order
        // change to async when code is implemented
        [HttpPut("{id}/items")]
        public IActionResult Items(int id)
        {
            // Is broken / confusing in Python version.
            return this.StatusCode(501);
        }

        // Deletes an order
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Order? order = this.orderService.GetOrderById(id);
            if (order is null)
            {
                return this.NotFound("Order not found");
            }

            await this.orderService.DeleteOrder(order);
            return this.Ok();
        }
    }
}
