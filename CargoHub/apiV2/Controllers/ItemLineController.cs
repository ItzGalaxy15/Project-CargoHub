using Microsoft.AspNetCore.Mvc;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/item_lines")]
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
            if (itemLine.Id != id)
            {
                return this.BadRequest("Invalid itemLine Id");
            }

            if (!this.itemLineValidationService.IsItemLineValid(itemLine, true))
            {
                return this.BadRequest("Invalid itemLine object");
            }

            ItemLine? old_itemLine = this.itemLineService.GetItemLineById(id);
            itemLine.CreatedAt = old_itemLine!.CreatedAt;
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

        // Patches an item line
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchItemLine(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            if (patch is null || !patch.Any())
            {
                return this.BadRequest("Invalid patch");
            }

            ItemLine? itemLine = this.itemLineService.GetItemLineById(id);
            if (itemLine == null)
            {
                return this.NotFound();
            }

            bool isValid = this.itemLineValidationService.IsItemLineValidForPATCH(patch);
            if (!isValid)
            {
                return this.BadRequest("Invalid patch");
            }

            await this.itemLineService.PatchItemLine(id, patch, itemLine);
            return this.Ok();
        }
    }
}
