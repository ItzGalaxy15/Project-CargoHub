using Microsoft.AspNetCore.Mvc;

[Route("api/v1/transfers")]
public class TransferController : Controller
{
    ITransferService _transferService;

    public TransferController(ITransferService transferService)
    {
        _transferService = transferService;
    }


    [HttpGet]
    public async Task<IActionResult> GetTransfers()
    {
        return Ok(_transferService.GetTransfers());
    }
}
