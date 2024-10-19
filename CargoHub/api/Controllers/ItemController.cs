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

    [HttpGet("{uid}")]
    public async Task<IActionResult> GetItemById(string uid)
    {
        Item? item = _itemService.GetItemById(uid);
        if (item == null)
        {
            return BadRequest();
        }
        return Ok(item);
    }
}
