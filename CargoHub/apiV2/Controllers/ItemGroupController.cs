using Microsoft.AspNetCore.Mvc;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;


namespace apiV2.Controllers
{
    [Route("api/v2/item_groups")]
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
            return itemGroup is null ? BadRequest() : Ok(itemGroup);
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

            if (!_itemGroupValidationService.IsItemGroupValid(itemGroup)) return BadRequest("invalid itemGroup object");
            await _itemGroupService.AddItemGroup(itemGroup);
            return  CreatedAtAction(nameof(GetItemGroupById), new { id = itemGroup.Id }, itemGroup);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceItemGroup([FromBody] ItemGroup itemGroup, int id)
        {
            if (itemGroup?.Id != id) return BadRequest("Invalid itemGroup Id");
            if (!_itemGroupValidationService.IsItemGroupValid(itemGroup, true)) return BadRequest("invalid itemGroup object");
            ItemGroup? oldItemGroup = _itemGroupService.GetItemGroupById(id);
            itemGroup.CreatedAt = oldItemGroup!.CreatedAt;
            await _itemGroupService.ReplaceItemGroup(itemGroup, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemGroup(int id)
        {
            ItemGroup? itemGroup = _itemGroupService.GetItemGroupById(id);
            if (itemGroup is null) return BadRequest();
            await _itemGroupService.DeleteItemGroup(itemGroup);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ModifyItemGroup(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            if (patch == null || !patch.Any())
                return BadRequest("No data provided for update.");

            ItemGroup? itemGroup = _itemGroupService.GetItemGroupById(id);
            if (itemGroup == null)
                return NotFound($"ItemGroup with ID {id} not found.");

            bool isValid = _itemGroupValidationService.IsItemGroupValidForPatch(patch);
            if (!isValid)
                return BadRequest("Invalid properties in the patch data.");

            await _itemGroupService.ModifyItemGroup(id, patch, itemGroup);
            return Ok();
        }
    }
}