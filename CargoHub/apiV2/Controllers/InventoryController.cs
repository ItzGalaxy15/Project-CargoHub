using Microsoft.AspNetCore.Mvc;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/inventories")]
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
            return inventory is null ? this.NotFound($"Inventory with ID {id} not found.") : this.Ok(inventory);
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
                return this.NotFound($"Inventory with ID {id} not found.");
            }

            await this.inventoryService.DeleteInventory(inventory);
            return this.Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ModifyInventory(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            if (patch == null || !patch.Any())
            {
                return this.BadRequest("No data provided for update.");
            }

            Inventory? inventory = this.inventoryService.GetInventoryById(id);
            if (inventory == null)
            {
                return this.NotFound($"Inventory with ID {id} not found.");
            }

            bool isValid = this.inventoryValidationService.IsInventoryValidForPatch(patch);
            if (!isValid)
            {
                return this.BadRequest("Invalid properties in the patch data.");
            }

            await this.inventoryService.ModifyInventory(id, patch, inventory);
            return this.Ok();
        }
    }
}