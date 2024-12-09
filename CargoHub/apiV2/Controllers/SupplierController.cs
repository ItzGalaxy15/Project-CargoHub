using Microsoft.AspNetCore.Mvc;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/suppliers")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService supplierService;
        private readonly ISupplierValidationService supplierValidationService;
        private readonly IItemService itemService;

        public SupplierController(ISupplierService supplierService, ISupplierValidationService supplierValidationService, IItemService itemService)
        {
            this.supplierService = supplierService;
            this.supplierValidationService = supplierValidationService;
            this.itemService = itemService;
        }

        // Get all suppliers
        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            Supplier[] suppliers = await Task.Run(() => this.supplierService.GetSuppliers());
            return this.Ok(suppliers);
        }

        // Get supplier by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            Supplier? supplier = await Task.Run(() => this.supplierService.GetSupplierById(id));
            return supplier is null ? this.NotFound() : this.Ok(supplier);
        }

        // Get supplier items
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetSupplierItems(int id)
        {
            // Maybe check if supplier exists?
            Item[] items = await Task.Run(() => this.itemService.GetItemsFromSupplierId(id));
            return this.Ok(items);
        }

        // Add supplier
        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] Supplier supplier)
        {
            if (!this.supplierValidationService.IsSupplierValid(supplier))
            {
                return this.BadRequest("Invalid supplier object");
            }

            await this.supplierService.AddSupplier(supplier);
            return this.CreatedAtAction(nameof(this.GetSupplierById), new { id = supplier.Id }, supplier);
        }

        // Replace supplier
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceSupplier([FromBody] Supplier supplier, int id)
        {
            if (supplier?.Id != id)
            {
                return this.BadRequest("Invalid id");
            }

            if (!this.supplierValidationService.IsSupplierValid(supplier, true))
            {
                return this.BadRequest("Invalid supplier object");
            }

            Supplier? oldSupplier = this.supplierService.GetSupplierById(id);
            supplier.CreatedAt = oldSupplier!.CreatedAt;
            await this.supplierService.ReplaceSupplier(supplier, id);
            return this.Ok();
        }

        // Delete supplier
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            Supplier? supplier = this.supplierService.GetSupplierById(id);
            if (supplier is null)
            {
                return this.BadRequest("Supplier not found");
            }

            await this.supplierService.DeleteSupplier(supplier);
            return this.Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ModifySupplier(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            if (patch == null || !patch.Any())
            {
                return this.BadRequest("No data provided for update.");
            }

            Supplier? supplier = this.supplierService.GetSupplierById(id);
            if (supplier == null)
            {
                return this.NotFound($"Supplier with ID {id} not found.");
            }

            bool isValid = this.supplierValidationService.IsSupplierValidForPatch(patch);
            if (!isValid)
            {
                return this.BadRequest("Invalid properties in the patch data.");
            }

            await this.supplierService.ModifySupplier(id, patch, supplier);
            return this.Ok();
        }
    }
}