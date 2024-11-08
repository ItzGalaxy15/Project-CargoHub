using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<IActionResult> GetInventories()
    {
        return Ok(_inventoryService.GetInventories());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInventoryById(int id)
    {
        var inventory = _inventoryService.GetInventoryById(id);
        return inventory is null ? BadRequest() : Ok(inventory);
    }

    [HttpPost]
    public async Task<IActionResult> AddInventory([FromBody] Inventory inventory)
    {
        if (!_inventoryValidationService.IsInventoryValid(inventory)) return BadRequest("invalid inventory object");
        await _inventoryService.AddInventory(inventory);
        return  CreatedAtAction(nameof(GetInventoryById), new { id = inventory.Id }, inventory);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceInventory([FromBody] Inventory inventory, int id)
    {
        if (inventory?.Id != id) return BadRequest("Invalid inventory Id");
        if (!_inventoryValidationService.IsInventoryValid(inventory, true)) return BadRequest("invalid inventory object");
        Inventory? oldInventory = _inventoryService.GetInventoryById(id);
        inventory.CreatedAt = oldInventory!.CreatedAt;
        await _inventoryService.ReplaceInventory(inventory, id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventory(int id)
    {
        Inventory? inventory = _inventoryService.GetInventoryById(id);
        if (inventory is null) return BadRequest();
        await _inventoryService.DeleteInventory(inventory);
        return Ok();
    }
}