using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/item_lines")]
    public class ItemLineController : Controller
    {
        private readonly IItemLineService itemLineService;
        private readonly IItemService itemService;
        private readonly IItemLineValidationService itemLineValidationService;

        public ItemLineController(IItemLineService itemLineService, IItemService itemService, IItemLineValidationService itemLineValidationService)
        {
            this.itemLineService = itemLineService;
            this.itemService = itemService;
            this.itemLineValidationService = itemLineValidationService;
        }

        // Get all item lines
        [HttpGet]
        public async Task<IActionResult> GetItemLines()
        {
            var itemLines = await Task.Run(() => this.itemLineService.GetItemLines());
            return this.Ok(itemLines);
        }

        // Get item line by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemLineById(int id)
        {
            ItemLine? itemLine = await Task.Run(() => this.itemLineService.GetItemLineById(id));

            // ItemLine? itemLine = _itemLineService.GetItemLineById(id);
            if (itemLine == null)
            {
                return this.NotFound();
            }

            return this.Ok(itemLine);
        }

        // Get items from item line
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetItemsFromItemLines(int id)
        {
            Item[] itemLineItems = await Task.Run(() => this.itemService.GetItemsFromItemLines(id));

            // Item[] items = _itemService.GetItemsFromItemLines(id);
            return this.Ok(itemLineItems);
        }

        // Adds an item line
        [HttpPost]
        public async Task<IActionResult> AddItemLine([FromBody] ItemLine itemLine)
        {
            if (!this.itemLineValidationService.IsItemLineValid(itemLine, false))
            {
                return this.BadRequest("Invalid itemLine object");
            }

            await this.itemLineService.AddItemLine(itemLine);
            return this.CreatedAtAction(nameof(this.GetItemLineById), new { id = itemLine.Id }, itemLine);
        }

        // Replaces an item line with a new one
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceItemLine(int id, [FromBody] ItemLine itemLine)
        {
            ItemLine? existingItemLine = this.itemLineService.GetItemLineById(id);

            // return badrequest if given id does not match any item line id
            ItemLine? old_itemLine = this.itemLineService.GetItemLineById(id);
            itemLine.CreatedAt = old_itemLine!.CreatedAt;
            if (existingItemLine == null || existingItemLine.Id != id)
            {
                return this.NotFound();
            }

            if (!this.itemLineValidationService.IsItemLineValid(itemLine, true))
            {
                return this.BadRequest("Invalid itemLine object");
            }

            await this.itemLineService.ReplaceItemLine(id, itemLine);
            return this.Ok();
        }

        // Deletes an item line
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemLine(int id)
        {
            ItemLine? itemLine = this.itemLineService.GetItemLineById(id);
            if (itemLine == null)
            {
                return this.NotFound();
            }

            await this.itemLineService.DeleteItemLine(itemLine);
            return this.Ok();
        }
    }
}
