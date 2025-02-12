using Microsoft.AspNetCore.Mvc;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/orders")]
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

            await this.orderService.UpdateOrder(order, id);
            return this.Ok();
        }

        // Updates items in an order
        [HttpPut("{id}/items")]

        public async Task<IActionResult> Items(int id, [FromBody] ItemSmall[] items)
        {
            Order? order = this.orderService.GetOrderById(id);
            if (order is null)
            {
                return this.NotFound();
            }

            await this.orderService.UpdateItemsInOrder(order, items, id);
            return this.Ok(items);
        }

        // Deletes an order
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Order? order = this.orderService.GetOrderById(id);
            if (order is null)
            {
                return this.NotFound($"Order with ID {id} not found.");
            }

            await this.orderService.DeleteOrder(order);
            return this.Ok();
        }

        // Patches an order with specific fields
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchOrder(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            bool isValid = this.orderValidationService.IsOrderValidForPATCH(patch, id);
            if (!isValid)
            {
                return this.BadRequest("Invalid patch");
            }

            Order? order = await Task.Run(() => this.orderService.GetOrderById(id));
            await this.orderService.PatchOrder(id, patch, order!);
            return this.Ok();
        }

        [HttpPut("{id}/items/{itemId}")]
        public async Task<IActionResult> UpdateItemInOrderAndShipment(int id, string itemId, [FromBody] ItemSmall updatedItem)
        {
            if (updatedItem.ItemId != itemId)
            {
                return this.BadRequest("Item ID mismatch");
            }

            try
            {
                await this.orderService.UpdateItemInOrderAndShipment(id, updatedItem);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
