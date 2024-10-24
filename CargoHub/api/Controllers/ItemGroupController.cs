using Microsoft.AspNetCore.Mvc;

[Route("api/v1/item_groups")]
public class ItemGroupController : Controller
{
    private readonly IItemGroupService _itemGroupService;
    private readonly IItemService _itemService;

    public ItemGroupController(IItemGroupService itemGroupService, IItemService itemService)
    {
        _itemGroupService = itemGroupService;
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetItemGroups()
    {
        return Ok(_itemGroupService.GetItemGroups());
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemGroupById(int id)
    {
        var itemGroup = _itemGroupService.GetItemGroupById(id);
        return itemGroup is null ? BadRequest() : Ok(itemGroup);
    }

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetItemsForItemGroups(int id) // id = itemGroupId
    {
        Item[] items = _itemService.GetItemsForItemGroups(id);
        return Ok(items);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceItemGroup([FromBody] ItemGroup itemGroup, int id)
    {
        bool result = await _itemGroupService.ReplaceItemGroup(itemGroup, id);
        return result ? Ok() : BadRequest("itemGroup not found");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItemGroup(int id)
    {
        ItemGroup? itemGroup = _itemGroupService.GetItemGroupById(id);
        if (itemGroup is null) return BadRequest();
        await _itemGroupService.DeleteItemGroup(itemGroup);
        return Ok();
    }

}
