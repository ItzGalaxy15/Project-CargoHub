using Microsoft.AspNetCore.Mvc;
using apiV2.Interfaces;
using apiV2.ValidationInterfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/transfers")]
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
                return this.NotFound($"Transfer with ID {id} not found.");
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
        public async Task<IActionResult> ReplaceTransfer([FromBody] Transfer transfer, int id)
        {
            if (transfer.Id != id)
            {
                return this.BadRequest("Invalid transfer Id");
            }

            if (!this.transferValidationService.IsTransferValid(transfer, true))
            {
                return this.BadRequest("Invalid transfer object");
            }

            Transfer? oldTransfer = this.transferService.GetTransferById(id);
            transfer.CreatedAt = oldTransfer!.CreatedAt;
            await this.transferService.UpdateTransfer(transfer, id);
            return this.Ok();
        }

        // NOT YET IMPLEMENTED
        // change to async when code is implemented
        [HttpPut("{id}/commit")]
        public async Task<IActionResult> Commit(int id)
        {
            Transfer? transfer = this.transferService.GetTransferById(id);
            if (transfer == null)
            {
                return this.NotFound();
            }

            await this.transferService.CommitTransfer(id);
            return this.Ok(transfer);
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTransfer(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            bool isValid = this.transferValidationService.IsTransferValidForPATCH(patch, id);
            if (!isValid)
            {
                return this.BadRequest("Invalid patch");
            }

            Transfer? transfer = await Task.Run(() => this.transferService.GetTransferById(id));
            await this.transferService.PatchTransfer(id, patch, transfer!);
            return this.Ok();
        }
    }
}