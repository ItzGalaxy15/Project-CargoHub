using Microsoft.AspNetCore.Mvc;

[Route("api/v1/item_lines")]
public class ItemLineController : Controller
{
    private readonly IItemLineService _itemLineService;
    private readonly IItemService _itemService;
    private readonly IItemLineValidationService _itemLineValidationService;

    public ItemLineController(IItemLineService itemLineService, IItemService itemService, IItemLineValidationService itemLineValidationService)
    {
        _itemLineService = itemLineService;
        _itemService = itemService;
        _itemLineValidationService = itemLineValidationService;
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
    public async Task<IActionResult> GetItemsFromItemLines(int id)
    {
        Item[] items = _itemService.GetItemsFromItemLines(id);
        return Ok(items);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceItemLine(int id, [FromBody] ItemLine itemLine)
    {
        ItemLine? existingItemLine = _itemLineService.GetItemLineById(id);
        //return badrequest if given id does not match any item line id
        if (existingItemLine == null || existingItemLine.Id != id)
        {
            return BadRequest();
        }
        if (!_itemLineValidationService.IsItemLineValid(itemLine, true))
        {
            return BadRequest("Invalid itemLine object");
        }
        await _itemLineService.ReplaceItemLine(id, itemLine);
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