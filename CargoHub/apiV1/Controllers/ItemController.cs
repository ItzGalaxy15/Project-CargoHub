using System.Xml;
using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/items")]
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
        public async Task<IActionResult> GetItems()
        {
            var items = await Task.Run(() => this.itemService.GetItems());
            return this.Ok(items);
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
        public async Task<IActionResult> ReplaceItem([FromBody] Item item, string uid)
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
    }
}