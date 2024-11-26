using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;


namespace apiV1.Controllers
{
    [Route("api/v1/inventories")]
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryValidationService _inventoryValidationService;

        public InventoryController(IInventoryService inventoryService, IInventoryValidationService inventoryValidationService)
        {
            _inventoryService = inventoryService;
            _inventoryValidationService = inventoryValidationService;
        }

        // Returns all inventories
        [HttpGet]
        public async Task<IActionResult> GetInventories()
        {
            Inventory[] inventories = await Task.Run(() => _inventoryService.GetInventories());
            return Ok(inventories);
        }

        // Returns an inventory by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            Inventory? inventory = await Task.Run(() => _inventoryService.GetInventoryById(id));
            return inventory is null ? NotFound() : Ok(inventory);
        }

        // Adds a new inventory
        [HttpPost]
        public async Task<IActionResult> AddInventory([FromBody] Inventory inventory)
        {
            if (!_inventoryValidationService.IsInventoryValid(inventory)) return NotFound("invalid inventory object");
            await _inventoryService.AddInventory(inventory);
            return  CreatedAtAction(nameof(GetInventoryById), new { id = inventory.Id }, inventory);
        }

        // Replaces an inventory with a new one
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceInventory([FromBody] Inventory inventory, int id)
        {
            if (inventory?.Id != id) return NotFound("Invalid inventory Id");
            if (!_inventoryValidationService.IsInventoryValid(inventory, true)) return NotFound("invalid inventory object");
            Inventory? oldInventory = _inventoryService.GetInventoryById(id);
            inventory.CreatedAt = oldInventory!.CreatedAt;
            await _inventoryService.ReplaceInventory(inventory, id);
            return Ok();
        }

        // Deletes an inventory
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            Inventory? inventory = _inventoryService.GetInventoryById(id);
            if (inventory is null) return NotFound();
            await _inventoryService.DeleteInventory(inventory);
            return Ok();
        }
    }
}