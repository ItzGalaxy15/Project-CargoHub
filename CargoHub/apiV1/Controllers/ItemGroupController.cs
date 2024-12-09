using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/item_groups")]
    public class ItemGroupController : Controller
    {
        private readonly IItemGroupService itemGroupService;
        private readonly IItemService itemService;
        private readonly IItemGroupValidationService itemGroupValidationService;

        public ItemGroupController(IItemGroupService itemGroupService, IItemService itemService,
        IItemGroupValidationService itemGroupValidationService)
        {
            this.itemGroupService = itemGroupService;
            this.itemService = itemService;
            this.itemGroupValidationService = itemGroupValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItemGroups()
        {
            ItemGroup[] itemGroups = await Task.Run(() => this.itemGroupService.GetItemGroups());
            return this.Ok(itemGroups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemGroupById(int id)
        {
            ItemGroup? itemGroup = await Task.Run(() => this.itemGroupService.GetItemGroupById(id));
            return itemGroup is null ? this.NotFound() : this.Ok(itemGroup);
        }

        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetItemsForItemGroups(int id) // id = itemGroupId
        {
            Item[] items = await Task.Run(() => this.itemService.GetItemsForItemGroups(id));
            return this.Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemGroup([FromBody] ItemGroup itemGroup)
        {
            if (!this.itemGroupValidationService.IsItemGroupValid(itemGroup))
            {
                return this.BadRequest("invalid itemGroup object");
            }

            await this.itemGroupService.AddItemGroup(itemGroup);
            return this.CreatedAtAction(nameof(this.GetItemGroupById), new { id = itemGroup.Id }, itemGroup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceItemGroup([FromBody] ItemGroup itemGroup, int id)
        {
            if (itemGroup?.Id != id)
            {
                return this.BadRequest("Invalid itemGroup Id");
            }

            if (!this.itemGroupValidationService.IsItemGroupValid(itemGroup, true))
            {
                return this.BadRequest("invalid itemGroup object");
            }

            ItemGroup? oldItemGroup = this.itemGroupService.GetItemGroupById(id);
            itemGroup.CreatedAt = oldItemGroup!.CreatedAt;
            await this.itemGroupService.ReplaceItemGroup(itemGroup, id);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemGroup(int id)
        {
            ItemGroup? itemGroup = this.itemGroupService.GetItemGroupById(id);
            if (itemGroup is null)
            {
                return this.NotFound();
            }

            await this.itemGroupService.DeleteItemGroup(itemGroup);
            return this.Ok();
        }
    }
}