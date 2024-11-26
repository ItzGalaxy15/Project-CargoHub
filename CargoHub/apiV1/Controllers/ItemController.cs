using System.Xml;
using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/items")]
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
                return BadRequest();
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
                return BadRequest("Item not found");
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
                return BadRequest("Item not found");
            }
            return Ok(inventory);
        }

        // NEW ITEM
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] Item item)
        {
            if (!_itemValidationService.IsItemValid(item))
            {
                return BadRequest("Invalid item object");
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
                return BadRequest("Item id not correct");
            }
            if (!_itemValidationService.IsItemValid(item))
            {
                return BadRequest("Invalid item object");
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
                return BadRequest("Item not found");
            }
            await _itemService.DeleteItem(item);
            return Ok();
        }
    }
}