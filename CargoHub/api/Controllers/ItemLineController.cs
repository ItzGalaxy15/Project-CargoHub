using Microsoft.AspNetCore.Mvc;

[Route("api/v1/item_lines")]
public class ItemLineController : Controller
{
    private readonly IItemLineService _itemLineService;

    public ItemLineController(IItemLineService itemLineService)
    {
        _itemLineService = itemLineService;
    }

    [HttpGet]
    public async Task<IActionResult> GetItemLines()
    {
        return Ok(_itemLineService.GetItemLines());
    }
}