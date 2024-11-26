using System.Xml;
using Microsoft.AspNetCore.Mvc;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/items")]
    public class ItemController : Controller
    {
        IItemService _itemService;
        IInventoryService _inventoryService;
        IItemValidationService _itemValidationService;

        public ItemController(IItemService itemService, IInventoryService inventoryService, IItemValidationService itemValidationService)
        {
            _itemService = itemService;
            _inventoryService = inventoryService;
            _itemValidationService = itemValidationService;

        }


        // GET ALL ITEMS
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await Task.Run (() => _itemService.GetItems());
            return Ok(items);
        }



        // GET ITEM BY ID
        [HttpGet("{uid}")]
        public async Task<IActionResult> GetItemById(string uid)
        {
            Item? item = await Task.Run (() => _itemService.GetItemById(uid));
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


        // GET ITEM TOTALS BY UID
        [HttpGet("{uid}/inventory/totals")]
        public async Task<IActionResult> GetTotalsByUid(string uid)
        {
            var totals = await _inventoryService.GetItemStorageTotalsByUid(uid);
            if (totals == null)
            {
                return NotFound("Item not found");
            }
            
            return Ok(totals);
        }


        // GET INVENTORY BY UID
        [HttpGet("{uid}/inventory")]
        public async Task<IActionResult> GetInventoryByUid(string uid)
        {
            var inventory = await _inventoryService.GetInventoryByUid(uid);
            if (inventory == null)
            {
                return NotFound("Item not found");
            }
            return Ok(inventory);
        }

        // NEW ITEM
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] Item item)
        {
            if (!_itemValidationService.IsItemValid(item))
            {
                return NotFound("Invalid item object");
            }
            await _itemService.AddItem(item);
            return CreatedAtAction(nameof(GetItemById), new { uid = item.Uid }, item);
        }


        // UPDATE ITEM
        [HttpPut("{uid}")]
        public async Task<IActionResult> ReplaceItem([FromBody] Item item)
        {
            Item? existingItem = _itemService.GetItemById(item.Uid);
            Item? oldItem = _itemService.GetItemById(item.Uid);
            item.CreatedAt = oldItem!.CreatedAt;


            if (existingItem == null || existingItem.Uid != item.Uid)
            {
                return NotFound("Item id not correct");
            }
            if (!_itemValidationService.IsItemValid(item))
            {
                return NotFound("Invalid item object");
            }
            await _itemService.ReplaceItem(item);
            return Ok();
        }


        // DELETE ITEM BY ID
        [HttpDelete("{uid}")]
        public async Task<IActionResult> DeleteItem(string uid)
        {
            Item? item = _itemService.GetItemById(uid);
            if (item == null)
            {
                return NotFound("Item not found");
            }
            await _itemService.DeleteItem(item);
            return Ok();
        }


        // Can edit an item with PATCH request
        [HttpPatch("{uid}")]
        public async Task<IActionResult> PatchItem(string uid, [FromBody] Dictionary<string, dynamic> patch)
        {
            bool isValid = await _itemValidationService.IsItemValidForPATCH(patch, uid);
            if (!isValid)
            {
                return NotFound("Invalid patch");
            }

            Item? item = _itemService.GetItemById(uid);
            await _itemService.PatchItem(uid, patch, item!);
            return Ok();
        }


    }
}