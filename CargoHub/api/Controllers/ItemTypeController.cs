using Microsoft.AspNetCore.Mvc;

[Route("api/v1/item_types")]
public class ItemTypeController : Controller
{
    private readonly IItemTypeService _itemTypeService;
    private readonly IItemService _itemService;

    public ItemTypeController(IItemTypeService itemTypeService, IItemService itemService)
    {
        _itemTypeService = itemTypeService;
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetItemTypes()
    {
        ItemType[] itemTypes = await _itemTypeService.GetItemTypes();
        return Ok(itemTypes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemTypeById(int id)
    {
        ItemType? itemType = await _itemTypeService.GetItemTypeById(id);
        if (itemType == null) return NotFound();
        return Ok(itemType);
    }

    [HttpGet("{id}/items")]
    public IActionResult GetOrdersFromOrForItemType(int id)
    {
        Item[] items = _itemService.GetItems();
        Item[] correctItems = items.Where(i => i.ItemType == id).ToArray();
        if (!correctItems.Any()) return NotFound();
        return Ok(correctItems);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItemType(int id, [FromBody] ItemType updatedItemType)
    {
        bool check = await _itemTypeService.UpdateItemType(id, updatedItemType);
        if (!check) return NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItemType(int id){
        bool check = await _itemTypeService.DeleteItemType(id);
        if (!check) return NotFound();
        return Ok();
    }
}
