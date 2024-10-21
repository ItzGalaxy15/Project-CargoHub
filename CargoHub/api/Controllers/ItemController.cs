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

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] Item item)
    {
        bool result = await _itemService.AddItem(item);
        if (result == false)
        {
            return BadRequest("Item id already in use");
        }
        return Ok();
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

    [HttpGet("{uid}/inventory/totals")]
    public async Task<IActionResult> GetTotalsByUid(string uid)
    {
        var totals = await _itemService.GetItemStorageTotalsByUid(uid);
        if (totals == null)
        {
            return BadRequest("Item not found");
        }
        
        return Ok(totals);
    }

    [HttpGet("{uid}/inventory")]
    public async Task<IActionResult> GetInventoryByUid(string uid)
    {
        var inventory = await _itemService.GetInventoryByUid(uid);
        if (inventory == null)
        {
            return BadRequest("Item not found");
        }
        return Ok(inventory);
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

    [HttpPut("{uid}")]
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
