using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/shipments")]
    public class ShipmentController : Controller
    {
        private readonly IShipmentService shipmentService;
        private readonly IShipmentValidationService shipmentValidationService;
        private readonly IOrderService orderService;

        public ShipmentController(IShipmentService shipmentService, IShipmentValidationService shipmentValidationService, IOrderService orderService)
        {
            this.shipmentService = shipmentService;
            this.shipmentValidationService = shipmentValidationService;
            this.orderService = orderService;
        }

        // Returns all shipments
        [HttpGet]
        public IActionResult GetShipments()
        {
            return this.Ok(this.shipmentService.GetShipments());
        }

        // Returns a shipment by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipmentById(int id)
        {
            var shipment = await Task.Run(() => this.shipmentService.GetShipmentById(id));
            return shipment is null ? this.NotFound($"Shipment with ID {id} not found.") : this.Ok(shipment);
        }

        // Returns all items in a shipment
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetShipmentItems(int id)
        {
            Shipment? shipment = await Task.Run(() => this.shipmentService.GetShipmentById(id));
            if (shipment is null)
            {
                return this.NotFound();
            }

            ItemSmall[] items = this.shipmentService.GetShipmentItems(shipment);
            return this.Ok(items);
        }

        // Returns all orders related to a shipment
        [HttpGet("{id}/orders")]
        public async Task<IActionResult> GetOrderIdsRelatedToShipment(int id)
        {
            int[] orderIds = await Task.Run(() => this.orderService.GetOrderIdsRelatedToShipment(id));
            return this.Ok(orderIds);
        }

        // Adds a new shipment
        [HttpPost]
        public async Task<IActionResult> AddShipment([FromBody] Shipment shipment)
        {
            if (!this.shipmentValidationService.IsShipmentValid(shipment))
            {
                return this.BadRequest("Invalid shipment object");
            }

            await this.shipmentService.AddShipment(shipment);
            return this.CreatedAtAction(nameof(this.GetShipmentById), new { id = shipment.Id }, shipment);
        }

        // Replaces a shipment with a new one
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceShipment([FromBody] Shipment shipment, int id)
        {
            if (shipment?.Id != id)
            {
                return this.BadRequest("Invalid id");
            }

            if (!this.shipmentValidationService.IsShipmentValid(shipment, true))
            {
                return this.BadRequest("Invalid shipment object");
            }

            Shipment? oldShipment = this.shipmentService.GetShipmentById(id);
            shipment.CreatedAt = oldShipment!.CreatedAt;
            await this.shipmentService.ReplaceShipment(shipment, id);
            return this.Ok();
        }

        // Should probably become a PATCH request in v2
        [HttpPut("{id}/orders")]
        public async Task<IActionResult> UpdateOrdersInShipment(int id, [FromBody] int[] orderIds)
        {
            // Maybe check if shipment exists?
            bool result = await this.orderService.UpdateOrdersWithShipmentId(id, orderIds);
            return result ? this.Ok() : this.BadRequest("Invalid provided order id's"); // false not implemented yet
        }

        // change to async when code is implemented
        [HttpPut("{id}/items")]
        public IActionResult Items(int id)
        {
            // Is broken / confusing in Python version.
            return this.StatusCode(501);
        }

        // Not yet implemented
        // change to async when code is implemented
        [HttpPut("{id}/commit")]
        public IActionResult Commit(int id)
        {
            // Is broken / confusing in Python version.
            return this.StatusCode(501);
        }

        // Deletes a shipment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            Shipment? shipment = this.shipmentService.GetShipmentById(id);
            if (shipment is null)
            {
                return this.NotFound($"Shipment with ID {id} not found.");
            }

            await this.shipmentService.DeleteShipment(shipment);
            return this.Ok();
        }
    }
}
