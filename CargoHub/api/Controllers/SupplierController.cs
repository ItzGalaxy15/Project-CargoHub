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
}
