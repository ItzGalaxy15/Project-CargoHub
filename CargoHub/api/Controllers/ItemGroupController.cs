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
}
