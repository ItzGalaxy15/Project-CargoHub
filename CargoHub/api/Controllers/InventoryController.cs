using Microsoft.AspNetCore.Mvc;

[Route("api/v1/inventories")]
public class InventoryController : Controller
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
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
        bool result = await _inventoryService.AddInventory(inventory);
        return result ?  CreatedAtAction(nameof(GetInventoryById), new { id = inventory.Id }, inventory)
                        : BadRequest("inventory id already in use");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceInventory([FromBody] Inventory inventory, int id)
    {
        bool result = await _inventoryService.ReplaceInventory(inventory, id);
        return result ? Ok() : BadRequest("inventory not found");
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