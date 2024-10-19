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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransferById(int id)
    {
        Transfer? transfer = _transferService.GetTransferById(id);
        if (transfer == null)
        {
            return BadRequest();
        }
        return Ok(transfer);
    }
}
