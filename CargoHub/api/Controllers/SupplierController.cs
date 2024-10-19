using Microsoft.AspNetCore.Mvc;

[Route("api/v1/suppliers")]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;
    public SupplierController(ISupplierService supplierService){
        _supplierService = supplierService;
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
}
