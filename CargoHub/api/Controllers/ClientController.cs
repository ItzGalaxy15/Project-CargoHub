using Microsoft.AspNetCore.Mvc;

[Route("api/v1/clients")]
public class ClientController : Controller
{
    private readonly IClientService _clientService;
    private readonly IOrderService _orderService;
    public ClientController(IClientService clientService, IOrderService orderService){
        _clientService = clientService;
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClients(){
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


}
