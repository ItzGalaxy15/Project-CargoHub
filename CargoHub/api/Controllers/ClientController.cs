using Microsoft.AspNetCore.Mvc;

[Route("api/v1/clients")]
public class ClientController : Controller
{
    private readonly IClientService _clientService;
    public ClientController(IClientService clientService){
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClients(){
        Client[] clients = await _clientService.GetClients();
        return Ok(clients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(int id){
        Client? client = await _clientService.GetClientById(id);
        if (client == null) return BadRequest(); // dit moeten we zelf bedenken
        return Ok(client);
    }
    
}
