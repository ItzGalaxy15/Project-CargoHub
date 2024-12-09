using Microsoft.AspNetCore.Mvc;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;


namespace apiV2.Controllers
{
    [Route("api/v2/suppliers")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly ISupplierValidationService _supplierValidationService;
        private readonly IItemService _itemService;
        public SupplierController(ISupplierService supplierService, ISupplierValidationService supplierValidationService, IItemService itemService){
            _supplierService = supplierService;
            _supplierValidationService = supplierValidationService;
            _itemService = itemService;
        }

        // Get all suppliers
        [HttpGet]
        public async Task<IActionResult> GetSuppliers(){
            Supplier[] suppliers = await Task.Run(() => _supplierService.GetSuppliers());
            return Ok(suppliers);
        }

        // Get supplier by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id){
            Supplier? supplier = await Task.Run(() => _supplierService.GetSupplierById(id));
            return supplier is null ? NotFound() : Ok(supplier);
        }

        // Get supplier items
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetSupplierItems(int id){
            // Maybe check if supplier exists?
            Item[] items = await Task.Run(() => _itemService.GetItemsFromSupplierId(id));
            return Ok(items);
        }

        // Add supplier
        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] Supplier supplier){
            if (!_supplierValidationService.IsSupplierValid(supplier)) return BadRequest("Invalid supplier object");
            await _supplierService.AddSupplier(supplier);
            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, supplier);
        }

        // Replace supplier
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceSupplier([FromBody] Supplier supplier, int id){
            if (supplier?.Id != id) return BadRequest("Invalid id");
            if (!_supplierValidationService.IsSupplierValid(supplier, true)) return BadRequest("Invalid supplier object");
            Supplier? oldSupplier = _supplierService.GetSupplierById(id);
            supplier.CreatedAt = oldSupplier!.CreatedAt;
            await _supplierService.ReplaceSupplier(supplier, id);
            return Ok();
        }

        // Delete supplier
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id){
            Supplier? supplier = _supplierService.GetSupplierById(id);
            if (supplier is null) return BadRequest("Supplier not found");
            await _supplierService.DeleteSupplier(supplier);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ModifySupplier(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            if (patch == null || !patch.Any())
                return BadRequest("No data provided for update.");

            Supplier? supplier = _supplierService.GetSupplierById(id);
            if (supplier == null)
                return NotFound($"Supplier with ID {id} not found.");

            bool isValid = _supplierValidationService.IsSupplierValidForPatch(patch);
            if (!isValid)
                return BadRequest("Invalid properties in the patch data.");

            await _supplierService.ModifySupplier(id, patch, supplier);
            return Ok();
        }
    }
}