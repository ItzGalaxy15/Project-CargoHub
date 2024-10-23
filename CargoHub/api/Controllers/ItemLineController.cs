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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemLineById(int id)
    {
        ItemLine? itemLine = _itemLineService.GetItemLineById(id);
        if (itemLine == null)
        {
            return NotFound();
        }
        return Ok(itemLine);
    }

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetItemsByItemLineId(int id)
    {
        Item[] items = _itemLineService.GetItemsByItemLineId(id);
        if (items == null || items.Length == 0)
        {
            return NotFound();
        }
        return Ok(items);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceItemLine(int id, [FromBody] ItemLine newItemLine)
    {
        bool result = _itemLineService.ReplaceItemLine(id, newItemLine);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItemLine(int id)
    {
        ItemLine? itemLine = _itemLineService.GetItemLineById(id);
        if (itemLine == null)
        {
            return NotFound();
        }
        await _itemLineService.DeleteItemLine(itemLine);
        return Ok();
    }
}