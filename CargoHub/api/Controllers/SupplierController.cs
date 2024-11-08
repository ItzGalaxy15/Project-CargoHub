using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<IActionResult> GetSuppliers(){
        return Ok(_supplierService.GetSuppliers());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierById(int id){
        Supplier? supplier = _supplierService.GetSupplierById(id);
        return supplier is null ? BadRequest() : Ok(supplier);
    }

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetSupplierItems(int id){
        // Maybe check if supplier exists?
        Item[] items = _itemService.GetItemsFromSupplierId(id);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> AddSupplier([FromBody] Supplier supplier){
        if (!_supplierValidationService.IsSupplierValid(supplier)) return BadRequest("Invalid supplier object");
        await _supplierService.AddSupplier(supplier);
        return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, supplier);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id){
        Supplier? supplier = _supplierService.GetSupplierById(id);
        if (supplier is null) return BadRequest("Supplier not found");
        await _supplierService.DeleteSupplier(supplier);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ReplaceSupplier([FromBody] Supplier supplier, int id){
        if (supplier?.Id != id) return BadRequest("Invalid id");
        if (!_supplierValidationService.IsSupplierValid(supplier, true)) return BadRequest("Invalid supplier object");
        Supplier? oldSupplier = _supplierService.GetSupplierById(id);
        supplier.CreatedAt = oldSupplier!.CreatedAt;
        await _supplierService.ReplaceSupplier(supplier, id);
        return Ok();
    }
}
