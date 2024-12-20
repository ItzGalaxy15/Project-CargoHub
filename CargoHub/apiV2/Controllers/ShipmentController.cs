using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using apiV2.ValidationInterfaces;
using apiV2.Interfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/shipments")]
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
            Console.WriteLine("Hello V2");
            return this.Ok(this.shipmentService.GetShipments());
        }

        // Returns a shipment by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipmentById(int id)
        {
            var shipment = await Task.Run(() => this.shipmentService.GetShipmentById(id));
            return shipment is null ? this.NotFound() : this.Ok(shipment);
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

        [HttpPut("{id}/orders")]
        public async Task<IActionResult> UpdateOrdersInShipment(int id, [FromBody] int[] orderIds)
        {
            bool result = await this.orderService.UpdateOrdersWithShipmentId(id, orderIds);
            return result ? this.Ok() : this.BadRequest("Invalid provided order id's"); // false not implemented yet
        }

        [HttpPut("{id}/items")]
        public async Task<IActionResult> Items(int id, [FromBody] ItemSmall[] items)
        {
            Shipment? shipment = this.shipmentService.GetShipmentById(id);
            if (shipment is null)
            {
                return this.NotFound();
            }

            if (!this.shipmentValidationService.isItemSmallValid(items))
            {
                return this.BadRequest("Invalid item object");
            }

            await this.shipmentService.UpdateItemsInShipment(shipment, items, id);
            return this.Ok();
        }

        [HttpPut("{id}/commit")]
        public async Task<IActionResult> Commit(int id)
        {
            Shipment? shipment = this.shipmentService.GetShipmentById(id);
            if (shipment is null)
            {
                return this.NotFound();
            }

            if (!this.shipmentValidationService.IsShipmentCommitValid(shipment))
            {
                return this.BadRequest();
            }

            await this.shipmentService.CommitShipment(shipment);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            Shipment? shipment = this.shipmentService.GetShipmentById(id);
            if (shipment is null)
            {
                return this.NotFound();
            }

            await this.shipmentService.DeleteShipment(shipment);
            return this.Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchShipment(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            bool isValid = await this.shipmentValidationService.IsShipmentValidForPATCH(patch, id);
            if (!isValid)
            {
                return this.BadRequest();
            }

            Shipment? shipment = this.shipmentService.GetShipmentById(id);
            await this.shipmentService.PatchShipment(id, patch, shipment!);
            return this.Ok();
        }

        [HttpPatch("{id}/items")]
        public async Task<IActionResult> PatchItems(int id, [FromBody] ItemSmall patch)
        {
            bool isValid = await this.shipmentValidationService.IsShipmentItemValid(patch, id);
            if (!isValid)
            {
                return this.BadRequest();
            }

            Shipment? shipment = this.shipmentService.GetShipmentById(id);
            await this.shipmentService.PatchItemInShipment(shipment!, patch);
            return this.Ok();
        }

        [HttpPatch("{id}/commit")]
        public async Task<IActionResult> PatchCommit(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            bool isValid = await this.shipmentValidationService.IsShipmentValidForPATCH(patch, id);
            if (!isValid)
            {
                return this.BadRequest();
            }

            Shipment? shipment = this.shipmentService.GetShipmentById(id);
            await this.shipmentService.PatchShipment(id, patch, shipment!);
            return this.Ok();
        }
    }
}
