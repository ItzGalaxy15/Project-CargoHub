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

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetItemsByTransferId(int id)
    {
        ItemSmall[] items = _transferService.GetItemsByTransferId(id);
        if (items == null || items.Length == 0)
        {
            return NotFound();
        }
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> AddTransfer([FromBody] Transfer transfer)
    {
        bool result = await _transferService.AddTransfer(transfer);
        return result ? Ok() : BadRequest("Transfer id already in use");
    }

    [HttpPut("{id}/commit")]
    public async Task<IActionResult> Commit(int id){
        // Is broken in Python version, calls LocationId property, which doesnt exist.
        return StatusCode(501);
    }
}
