using System.Xml;
using Microsoft.AspNetCore.Mvc;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/items")]
    public class ItemController : Controller
    {
        private IItemService itemService;
        private IInventoryService inventoryService;
        private IItemValidationService itemValidationService;

        public ItemController(IItemService itemService, IInventoryService inventoryService, IItemValidationService itemValidationService)
        {
            this.itemService = itemService;
            this.inventoryService = inventoryService;
            this.itemValidationService = itemValidationService;
        }

        // GET ALL ITEMS
        [HttpGet]
        public IActionResult GetItems()
        {
            // Get filtered items from middleware
            var filteredItems = HttpContext.Items["FilteredItems"] as List<Item>;

            if (filteredItems != null)
            {
                Console.WriteLine($"Controller: Returning {filteredItems.Count} filtered items.");
                return Ok(filteredItems);
            }

            // Default behavior if no filtering is applied
            var items = this.itemService.GetItems();
            return Ok(items);
        }


        // GET ITEM BY ID
        [HttpGet("{uid}")]
        public async Task<IActionResult> GetItemById(string uid)
        {
            Item? item = await Task.Run(() => this.itemService.GetItemById(uid));
            if (item == null)
            {
                return this.NotFound();
            }

            return this.Ok(item);
        }

        // GET ITEM TOTALS BY UID
        [HttpGet("{uid}/inventory/totals")]
        public async Task<IActionResult> GetTotalsByUid(string uid)
        {
            var totals = await this.inventoryService.GetItemStorageTotalsByUid(uid);
            if (totals == null)
            {
                return this.NotFound("Item not found");
            }

            return this.Ok(totals);
        }

        // GET INVENTORY BY UID
        [HttpGet("{uid}/inventory")]
        public async Task<IActionResult> GetInventoryByUid(string uid)
        {
            var inventory = await this.inventoryService.GetInventoryByUid(uid);
            if (inventory == null)
            {
                return this.NotFound("Item not found");
            }

            return this.Ok(inventory);
        }

        // NEW ITEM
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] Item item)
        {
            if (!this.itemValidationService.IsItemValid(item))
            {
                return this.BadRequest("Invalid item object");
            }

            await this.itemService.AddItem(item);
            return this.CreatedAtAction(nameof(this.GetItemById), new { uid = item.Uid }, item);
        }

        // UPDATE ITEM
        [HttpPut("{uid}")]
        public async Task<IActionResult> UpdateItem([FromBody] Item item, string uid)
        {
            if (item.Uid != uid)
            {
                return this.BadRequest("Item id not correct");
            }

            if (!this.itemValidationService.IsItemValid(item, true))
            {
                return this.BadRequest("Invalid item object");
            }

            Item? oldItem = this.itemService.GetItemById(uid);
            item.CreatedAt = oldItem!.CreatedAt;

            await this.itemService.UpdateItem(item, uid);
            return this.Ok();
        }

        // DELETE ITEM BY ID
        [HttpDelete("{uid}")]
        public async Task<IActionResult> DeleteItem(string uid)
        {
            Item? item = this.itemService.GetItemById(uid);
            if (item == null)
            {
                return this.NotFound("Item not found");
            }

            await this.itemService.DeleteItem(item);
            return this.Ok();
        }

        // Can edit an item with PATCH request
        [HttpPatch("{uid}")]
        public async Task<IActionResult> PatchItem(string uid, [FromBody] Dictionary<string, dynamic> patch)
        {
            bool isValid = await this.itemValidationService.IsItemValidForPATCH(patch, uid);
            if (!isValid)
            {
                return this.BadRequest("Invalid patch");
            }

            Item? item = this.itemService.GetItemById(uid);
            await this.itemService.PatchItem(uid, patch, item!);
            return this.Ok();
        }
    }
}