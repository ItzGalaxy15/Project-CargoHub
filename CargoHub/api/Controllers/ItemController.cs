using System.Xml;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/items")]
public class ItemController : Controller
{
    IItemService _itemService;
    IInventoryService _inventoryService;

    public ItemController(IItemService itemService, IInventoryService inventoryService)
    {
        _itemService = itemService;
        _inventoryService = inventoryService;

    }


    // NEW ITEM
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


    // GET ALL ITEMS
    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        return Ok(_itemService.GetItems());
    }



    // GET ITEM BY ID
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


    // GET ITEM TOTALS BY UID
    [HttpGet("{uid}/inventory/totals")]
    public async Task<IActionResult> GetTotalsByUid(string uid)
    {
        var totals = await _inventoryService.GetItemStorageTotalsByUid(uid);
        if (totals == null)
        {
            return BadRequest("Item not found");
        }
        
        return Ok(totals);
    }


    // GET INVENTORY BY UID
    [HttpGet("{uid}/inventory")]
    public async Task<IActionResult> GetInventoryByUid(string uid)
    {
        var inventory = await _inventoryService.GetInventoryByUid(uid);
        if (inventory == null)
        {
            return BadRequest("Item not found");
        }
        return Ok(inventory);
    }


    // UPDATE ITEM
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


    // DELETE ITEM BY ID
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


}
