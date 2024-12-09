using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;
namespace apiV1.Controllers
{
    [Route("api/v1/transfers")]
    public class TransferController : Controller
    {
        ITransferService _transferService;
        ITransferValidationService _transferValidationService;

        public TransferController(ITransferService transferService, ITransferValidationService transferValidationService)
        {
            _transferService = transferService;
            _transferValidationService = transferValidationService;
        }


        // GETS ALL TRANSFERS
        [HttpGet]
        public async Task<IActionResult> GetTransfers()
        {
            var transfers = await Task.Run(() => _transferService.GetTransfers());
            return Ok(transfers);
        }


        // GET TRANSFER BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransferById(int id)
        {
            Transfer? transfer = await Task.Run (() => _transferService.GetTransferById(id));
            if (transfer == null)
            {
                return NotFound();
            }
            return Ok(transfer);
        }


        // GET ITEMS BY TRANSFER ID
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetItemsByTransferId(int id)
        {
            ItemSmall[] items = await Task.Run(() => _transferService.GetItemsByTransferId(id));

            if (items == null || items.Length == 0)
            {
                return NotFound();
            }
            return Ok(items);
        }


        // ADDS TRANSFER
        [HttpPost]
        public async Task<IActionResult> AddTransfer([FromBody] Transfer transfer)
        {
            if (!_transferValidationService.IsTransferValid(transfer))
            {
                return BadRequest("Invalid transfer object");
            }
            await _transferService.AddTransfer(transfer);
            return CreatedAtAction(nameof(GetTransferById), new { id = transfer.Id }, transfer);
        }


        //UPDATES TRANSFER BY ID
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceTransfer([FromBody] Transfer transfer, int Id)
        {
            
            if (transfer.Id != Id)
            {
                return BadRequest("Invalid Id");
            }
            if (!_transferValidationService.IsTransferValid(transfer, true))
            {
                return BadRequest("Invalid transfer object");
            }
            Transfer? oldTransfer = _transferService.GetTransferById(Id);     
            transfer.CreatedAt = oldTransfer!.CreatedAt;
            await _transferService.UpdateTransfer(transfer, Id);
            return Ok();
        }


        // NOT YET IMPLEMENTED
        // change to async when code is implemented
        [HttpPut("{id}/commit")]
        public IActionResult Commit(int id){
            // Is broken in Python version, calls LocationId property, which doesnt exist.
            return StatusCode(501);
        }


        // DELETE TRANSFER BY ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransfer(int id)
        {
            Transfer? transfer = _transferService.GetTransferById(id);
            if (transfer == null)
            {
                return NotFound();
            }
            await _transferService.DeleteTransfer(transfer);
            return Ok();
        }
    }
}