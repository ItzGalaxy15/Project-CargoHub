using System.Xml;
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

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] Item item)
    {
        bool result = await _itemService.AddItem(item);
        return result ? Ok() : BadRequest("Item id already in use");
        if (result == false)
        {
            return BadRequest("Item id already in use");
        }
        return Ok();
    }

    [HttpDelete("{uid}")]
    public async Task<IActionResult> DeleteItem(string uid)
    {
        Item? item = _itemService.GetItemById(uid);
        if (item == null)
        {
            return BadRequest("Item not found");
        }
        await _itemService.DeleteItem(item);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> ReplaceItem([FromBody] Item item)
    {
        bool result = await _itemService.ReplaceItem(item);
        if (result == false)
        {
            return BadRequest("Item not found");
        }
        return Ok();
    }
}
