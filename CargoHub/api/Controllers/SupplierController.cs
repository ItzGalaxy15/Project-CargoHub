using Microsoft.AspNetCore.Mvc;

[Route("api/v1/suppliers")]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;
    private readonly IItemService _itemService;
    public SupplierController(ISupplierService supplierService, IItemService itemService){
        _supplierService = supplierService;
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSuppliers(){
        return Ok(_supplierService.GetSuppliers());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplier(int id){
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
        bool result = await _supplierService.AddSupplier(supplier);
        return result ? Created() : BadRequest("Supplier id already in use");
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
        bool result = await _supplierService.ReplaceSupplier(supplier, id);
        return result ? Ok() : BadRequest("Supplier not found");
    }
}
