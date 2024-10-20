using Microsoft.AspNetCore.Mvc;

[Route("api/v1/item_types")]
public class ItemTypeController : Controller
{
    private readonly IItemTypeService _itemTypeService;

    public ItemTypeController(IItemTypeService itemTypeService)
    {
        _itemTypeService = itemTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetItemTypes()
    {
        ItemType[] itemTypes = await _itemTypeService.GetItemTypes();
        return Ok(itemTypes);
    }

}
