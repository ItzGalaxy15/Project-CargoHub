using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/item_types")]
    public class ItemTypeController : Controller
    {
        private readonly IItemTypeService itemTypeService;
        private readonly IItemTypeValidationService itemTypeValidationService;
        private readonly IItemService itemService;

        public ItemTypeController(IItemTypeService itemTypeService, IItemService itemService, IItemTypeValidationService itemTypeValidationService)
        {
            this.itemTypeService = itemTypeService;
            this.itemService = itemService;
            this.itemTypeValidationService = itemTypeValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItemTypes()
        {
            ItemType[] itemTypes = await this.itemTypeService.GetItemTypes();
            return this.Ok(itemTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemTypeById(int id)
        {
            ItemType? itemType = await this.itemTypeService.GetItemTypeById(id);
            if (itemType == null)
            {
                return this.NotFound();
            }

            return this.Ok(itemType);
        }

        [HttpGet("{id}/items")]
        public IActionResult GetItemsBasedOnItemTypes(int id)
        {
            Item[] items = this.itemService.GetItems();
            Item[] correctItems = items.Where(i => i.ItemType == id).ToArray();
            if (!correctItems.Any())
            {
                return this.NotFound();
            }

            return this.Ok(correctItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemType([FromBody] ItemType newItemType)
        {
            bool isValid = await this.itemTypeValidationService.IsItemTypeValidForPOST(newItemType);
            if (!isValid)
            {
                return this.BadRequest();
            }

            await this.itemTypeService.AddItemType(newItemType);
            return this.CreatedAtAction(nameof(this.GetItemTypeById), new { id = newItemType.Id }, newItemType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemType(int id, [FromBody] ItemType updatedItemType)
        {
            bool isValid = await this.itemTypeValidationService.IsItemTypeValidForPUT(updatedItemType, id);
            if (!isValid)
            {
                return this.BadRequest();
            }

            ItemType? oldItemType = await this.itemTypeService.GetItemTypeById(id);
            updatedItemType.CreatedAt = oldItemType!.CreatedAt;
            await this.itemTypeService.UpdateItemType(id, updatedItemType);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemType(int id)
        {
            ItemType? itemType = await this.itemTypeService.GetItemTypeById(id);
            if (itemType == null)
            {
                return this.NotFound();
            }

            await this.itemTypeService.DeleteItemType(itemType);
            return this.Ok();
        }
    }
}
