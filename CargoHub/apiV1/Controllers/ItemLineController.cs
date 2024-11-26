using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{    
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

        // Get all item lines
        [HttpGet]
        public async Task<IActionResult> GetItemLines()
        {
            var itemLines = await Task.Run(() => _itemLineService.GetItemLines());
            return Ok(itemLines);
        }

        // Get item line by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemLineById(int id)
        {
            ItemLine? itemLine = await Task.Run(() => _itemLineService.GetItemLineById(id));
            // ItemLine? itemLine = _itemLineService.GetItemLineById(id);
            if (itemLine == null)
            {
                return NotFound();
            }
            return Ok(itemLine);
        }

        // Get items from item line
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetItemsFromItemLines(int id)
        {   
            Item[] ItemLineItems = await Task.Run(() => _itemService.GetItemsFromItemLines(id));
            // Item[] items = _itemService.GetItemsFromItemLines(id);
            return Ok(ItemLineItems);
        }

        // Adds an item line
        [HttpPost]
        public async Task<IActionResult> AddItemLine([FromBody] ItemLine itemLine)
        {
            if (!_itemLineValidationService.IsItemLineValid(itemLine, false))
            {
                return BadRequest("Invalid itemLine object");
            }
            await _itemLineService.AddItemLine(itemLine);
            return CreatedAtAction(nameof(GetItemLineById), new { id = itemLine.Id }, itemLine);
        }

        // Replaces an item line with a new one
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceItemLine(int id, [FromBody] ItemLine itemLine)
        {
            ItemLine? existingItemLine = _itemLineService.GetItemLineById(id);
            //return badrequest if given id does not match any item line id
            ItemLine? old_itemLine = _itemLineService.GetItemLineById(id);
            itemLine.CreatedAt = old_itemLine!.CreatedAt;
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

        // Deletes an item line
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
}
