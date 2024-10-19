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

}
