using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/inventories")]
    public class InventoryController : Controller
    {
        private readonly IInventoryService inventoryService;
        private readonly IInventoryValidationService inventoryValidationService;

        public InventoryController(IInventoryService inventoryService, IInventoryValidationService inventoryValidationService)
        {
            this.inventoryService = inventoryService;
            this.inventoryValidationService = inventoryValidationService;
        }

        // Returns all inventories
        [HttpGet]
        public async Task<IActionResult> GetInventories()
        {
            Inventory[] inventories = await Task.Run(() => this.inventoryService.GetInventories());
            return this.Ok(inventories);
        }

        // Returns an inventory by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            Inventory? inventory = await Task.Run(() => this.inventoryService.GetInventoryById(id));
            return inventory is null ? this.NotFound() : this.Ok(inventory);
        }

        // Adds a new inventory
        [HttpPost]
        public async Task<IActionResult> AddInventory([FromBody] Inventory inventory)
        {
            if (!this.inventoryValidationService.IsInventoryValid(inventory))
            {
                return this.BadRequest("invalid inventory object");
            }

            await this.inventoryService.AddInventory(inventory);
            return this.CreatedAtAction(nameof(this.GetInventoryById), new { id = inventory.Id }, inventory);
        }

        // Replaces an inventory with a new one
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceInventory([FromBody] Inventory inventory, int id)
        {
            if (inventory?.Id != id)
            {
                return this.BadRequest("Invalid inventory Id");
            }

            if (!this.inventoryValidationService.IsInventoryValid(inventory, true))
            {
                return this.BadRequest("invalid inventory object");
            }

            Inventory? oldInventory = this.inventoryService.GetInventoryById(id);
            inventory.CreatedAt = oldInventory!.CreatedAt;
            await this.inventoryService.ReplaceInventory(inventory, id);
            return this.Ok();
        }

        // Deletes an inventory
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            Inventory? inventory = this.inventoryService.GetInventoryById(id);
            if (inventory is null)
            {
                return this.NotFound();
            }

            await this.inventoryService.DeleteInventory(inventory);
            return this.Ok();
        }
    }
}