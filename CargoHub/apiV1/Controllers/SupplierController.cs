using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;


namespace apiV1.Controllers
{
    [Route("api/v1/suppliers")]
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
            if (!_supplierValidationService.IsSupplierValid(supplier)) return NotFound("Invalid supplier object");
            await _supplierService.AddSupplier(supplier);
            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, supplier);
        }

        // Replace supplier
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceSupplier([FromBody] Supplier supplier, int id){
            if (supplier?.Id != id) return NotFound("Invalid id");
            if (!_supplierValidationService.IsSupplierValid(supplier, true)) return NotFound("Invalid supplier object");
            Supplier? oldSupplier = _supplierService.GetSupplierById(id);
            supplier.CreatedAt = oldSupplier!.CreatedAt;
            await _supplierService.ReplaceSupplier(supplier, id);
            return Ok();
        }

        // Delete supplier
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id){
            Supplier? supplier = _supplierService.GetSupplierById(id);
            if (supplier is null) return NotFound("Supplier not found");
            await _supplierService.DeleteSupplier(supplier);
            return Ok();
        }
    }
}