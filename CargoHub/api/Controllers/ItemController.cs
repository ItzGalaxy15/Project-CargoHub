using Microsoft.AspNetCore.Mvc;

[Route("api/v1/items")]
public class ItemController : Controller
{
    IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        return Ok(_itemService.GetItems());
    }
}
