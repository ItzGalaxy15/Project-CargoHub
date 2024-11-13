using Microsoft.AspNetCore.Mvc;
using api.ValidationInterface;
using apiV2.Interface;


namespace apiV2.Controllers
{
    [Route("api/v2/clients")]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IClientValidationService _clientValidationService;
        private readonly IOrderService _orderService;
        public ClientController(IClientService clientService, IOrderService orderService, IClientValidationService clientValidationService){
            _clientValidationService = clientValidationService;
            _clientService = clientService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients(){
            Console.WriteLine("Hello V2");
            Client[] clients = await _clientService.GetClients();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id){
            Client? client = await _clientService.GetClientById(id);
            if (client == null) return NotFound(); 
            return Ok(client);
        }

        [HttpGet("{id}/orders")]
        public async Task<IActionResult> GetOrdersFromOrForClient(int id){
            Order[] orders = await _orderService.GetOrders();
            Order[] correctOrders = orders.Where(o => o.ShipTo == id || o.BillTo == id).ToArray();
            if(!correctOrders.Any()) return NotFound();
            return Ok(correctOrders);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Client newClient){
            bool isValid = await _clientValidationService.IsClientValidForPOST(newClient);
            if (!isValid) return BadRequest();
            await _clientService.AddClient(newClient);
            return CreatedAtAction(nameof(GetClientById), new { id = newClient.Id }, newClient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] Client updatedClient){
            bool isValid = await _clientValidationService.IsClientValidForPUT(updatedClient, id);
            if (!isValid) return BadRequest();
            Client? oldClient = await _clientService.GetClientById(id);
            updatedClient.CreatedAt = oldClient!.CreatedAt;
            await _clientService.UpdateClient(id, updatedClient);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id){
            Client? client = await _clientService.GetClientById(id);
            if (client is null) return BadRequest();
            await _clientService.DeleteClient(client);
            return Ok();
        }

    }
}
