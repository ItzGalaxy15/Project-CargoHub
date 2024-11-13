using Microsoft.AspNetCore.Mvc;
using apiV2.Interface;
using api.ValidationInterface;

namespace apiV2.Controllers
{
    [Route("api/v2/item_types")]
    public class ItemTypeController : Controller
    {
        private readonly IItemTypeService _itemTypeService;
        private readonly IItemTypeValidationService _itemTypeValidationService;
        private readonly IItemService _itemService;

        public ItemTypeController(IItemTypeService itemTypeService, IItemService itemService, IItemTypeValidationService itemTypeValidationService)
        {
            _itemTypeService = itemTypeService;
            _itemService = itemService;
            _itemTypeValidationService = itemTypeValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItemTypes()
        {
            ItemType[] itemTypes = await _itemTypeService.GetItemTypes();
            Console.WriteLine("hello");
            return Ok(itemTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemTypeById(int id)
        {
            ItemType? itemType = await _itemTypeService.GetItemTypeById(id);
            if (itemType == null) return NotFound();
            return Ok(itemType);
        }

        [HttpGet("{id}/items")]
        public IActionResult GetItemsBasedOnItemTypes(int id)
        {
            Item[] items = _itemService.GetItems();
            Item[] correctItems = items.Where(i => i.ItemType == id).ToArray();
            if (!correctItems.Any()) return NotFound();
            return Ok(correctItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemType([FromBody] ItemType newItemType)
        {
            bool isValid = await _itemTypeValidationService.IsItemTypeValidForPOST(newItemType);
            if (!isValid) return BadRequest(); 
            await _itemTypeService.AddItemType(newItemType);
            return CreatedAtAction(nameof(GetItemTypeById), new { id = newItemType.Id }, newItemType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemType(int id, [FromBody] ItemType updatedItemType)
        {
            bool isValid = await _itemTypeValidationService.IsItemTypeValidForPUT(updatedItemType, id);
            if (!isValid) return BadRequest(); 
            ItemType? oldItemType = await _itemTypeService.GetItemTypeById(id);
            updatedItemType.CreatedAt = oldItemType!.CreatedAt;
            await _itemTypeService.UpdateItemType(id, updatedItemType);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemType(int id){
            ItemType? itemType = await _itemTypeService.GetItemTypeById(id);
            if (itemType == null) return BadRequest();
            await _itemTypeService.DeleteItemType(itemType);
            return Ok();
        }
    }
}
