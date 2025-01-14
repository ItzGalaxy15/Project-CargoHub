using Microsoft.AspNetCore.Mvc;
using apiV2.ValidationInterfaces;
using apiV2.Interfaces;

namespace apiV2.Controllers
{
    [Route("api/v2/clients")]
    public class ClientController : Controller
    {
        private readonly IClientService clientService;
        private readonly IClientValidationService clientValidationService;
        private readonly IOrderService orderService;

        public ClientController(IClientService clientService, IOrderService orderService, IClientValidationService clientValidationService)
        {
            this.clientValidationService = clientValidationService;
            this.clientService = clientService;
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            Client[] clients = await this.clientService.GetClients();
            return this.Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            Client? client = await this.clientService.GetClientById(id);
            if (client == null)
            {
                return this.NotFound($"Client with ID {id} not found.");
            }

            return this.Ok(client);
        }

        [HttpGet("{id}/orders")]
        public async Task<IActionResult> GetOrdersFromOrForClient(int id)
        {
            Order[] orders = await this.orderService.GetOrders();
            Order[] correctOrders = orders.Where(o => o.ShipTo == id || o.BillTo == id).ToArray();
            if (!correctOrders.Any())
            {
                return this.NotFound();
            }

            return this.Ok(correctOrders);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Client newClient)
        {
            bool isValid = await this.clientValidationService.IsClientValidForPOST(newClient);
            if (!isValid)
            {
                return this.BadRequest();
            }

            await this.clientService.AddClient(newClient);
            return this.CreatedAtAction(nameof(this.GetClientById), new { id = newClient.Id }, newClient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client updatedClient)
        {
            bool isValid = await this.clientValidationService.IsClientValidForPUT(updatedClient, id);
            if (!isValid)
            {
                return this.BadRequest();
            }

            Client? oldClient = await this.clientService.GetClientById(id);
            updatedClient.CreatedAt = oldClient!.CreatedAt;
            await this.clientService.UpdateClient(id, updatedClient);
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            Client? client = await this.clientService.GetClientById(id);
            if (client is null)
            {
                return this.NotFound($"Client with ID {id} not found.");
            }

            await this.clientService.DeleteClient(client);
            return this.Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchClient(int id, [FromBody] Dictionary<string, dynamic> patch)
        {
            bool isValid = await this.clientValidationService.IsClientValidForPATCH(patch, id);
            if (!isValid)
            {
                return this.BadRequest();
            }

            Client? client = await this.clientService.GetClientById(id);
            await this.clientService.PatchClient(id, patch, client!);
            return this.Ok();
        }
    }
}
