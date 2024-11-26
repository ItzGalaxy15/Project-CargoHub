using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;


namespace apiV1.Controllers
{
    [Route("api/v1/item_groups")]
    public class ItemGroupController : Controller
    {
        private readonly IItemGroupService _itemGroupService;
        private readonly IItemService _itemService;
        private readonly IItemGroupValidationService _itemGroupValidationService;

        public ItemGroupController(IItemGroupService itemGroupService, IItemService itemService,
        IItemGroupValidationService itemGroupValidationService)
        {
            _itemGroupService = itemGroupService;
            _itemService = itemService;
            _itemGroupValidationService = itemGroupValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItemGroups()
        {
            ItemGroup[] itemGroups = await Task.Run(() => _itemGroupService.GetItemGroups());
            return Ok(itemGroups);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemGroupById(int id)
        {
            
            ItemGroup? itemGroup = await Task.Run(() => _itemGroupService.GetItemGroupById(id));
            return itemGroup is null ? NotFound() : Ok(itemGroup);
        }

        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetItemsForItemGroups(int id) // id = itemGroupId
        {
            Item[] items = await Task.Run(() => _itemService.GetItemsForItemGroups(id));
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemGroup([FromBody] ItemGroup itemGroup)
        {

            if (!_itemGroupValidationService.IsItemGroupValid(itemGroup)) return NotFound("invalid itemGroup object");
            await _itemGroupService.AddItemGroup(itemGroup);
            return  CreatedAtAction(nameof(GetItemGroupById), new { id = itemGroup.Id }, itemGroup);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceItemGroup([FromBody] ItemGroup itemGroup, int id)
        {
            if (itemGroup?.Id != id) return NotFound("Invalid itemGroup Id");
            if (!_itemGroupValidationService.IsItemGroupValid(itemGroup, true)) return NotFound("invalid itemGroup object");
            ItemGroup? oldItemGroup = _itemGroupService.GetItemGroupById(id);
            itemGroup.CreatedAt = oldItemGroup!.CreatedAt;
            await _itemGroupService.ReplaceItemGroup(itemGroup, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemGroup(int id)
        {
            ItemGroup? itemGroup = _itemGroupService.GetItemGroupById(id);
            if (itemGroup is null) return NotFound();
            await _itemGroupService.DeleteItemGroup(itemGroup);
            return Ok();
        }
    }
}