using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/transfers")]
    public class TransferController : Controller
    {
        private ITransferService transferService;
        private ITransferValidationService transferValidationService;

        public TransferController(ITransferService transferService, ITransferValidationService transferValidationService)
        {
            this.transferService = transferService;
            this.transferValidationService = transferValidationService;
        }

        // GETS ALL TRANSFERS
        [HttpGet]
        public async Task<IActionResult> GetTransfers()
        {
            var transfers = await Task.Run(() => this.transferService.GetTransfers());
            return this.Ok(transfers);
        }

        // GET TRANSFER BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransferById(int id)
        {
            Transfer? transfer = await Task.Run(() => this.transferService.GetTransferById(id));
            if (transfer == null)
            {
                this.NotFound($"Transfer with ID {id} not found.");
            }

            return this.Ok(transfer);
        }

        // GET ITEMS BY TRANSFER ID
        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetItemsByTransferId(int id)
        {
            ItemSmall[] items = await Task.Run(() => this.transferService.GetItemsByTransferId(id));

            if (items == null || items.Length == 0)
            {
                return this.NotFound();
            }

            return this.Ok(items);
        }

        // ADDS TRANSFER
        [HttpPost]
        public async Task<IActionResult> AddTransfer([FromBody] Transfer transfer)
        {
            if (!this.transferValidationService.IsTransferValid(transfer))
            {
                return this.BadRequest("Invalid transfer object");
            }

            await this.transferService.AddTransfer(transfer);
            return this.CreatedAtAction(nameof(this.GetTransferById), new { id = transfer.Id }, transfer);
        }

        // UPDATES TRANSFER BY ID
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceTransfer([FromBody] Transfer transfer, int Id)
        {
            if (transfer.Id != Id)
            {
                return this.BadRequest("Invalid Id");
            }

            if (!this.transferValidationService.IsTransferValid(transfer, true))
            {
                return this.BadRequest("Invalid transfer object");
            }

            Transfer? oldTransfer = this.transferService.GetTransferById(Id);
            transfer.CreatedAt = oldTransfer!.CreatedAt;
            await this.transferService.UpdateTransfer(transfer, Id);
            return this.Ok();
        }

        // NOT YET IMPLEMENTED
        // change to async when code is implemented
        [HttpPut("{id}/commit")]
        public IActionResult Commit(int id)
        {
            // Is broken in Python version, calls LocationId property, which doesnt exist.
            return this.StatusCode(501);
        }

        // DELETE TRANSFER BY ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransfer(int id)
        {
            Transfer? transfer = this.transferService.GetTransferById(id);
            if (transfer == null)
            {
                return this.NotFound($"Transfer with ID {id} not found.");
            }

            await this.transferService.DeleteTransfer(transfer);
            return this.Ok();
        }
    }
}