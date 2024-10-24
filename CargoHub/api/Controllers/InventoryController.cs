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
}
