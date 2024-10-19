public class ClientService : IClientService
{
    private readonly IClientProvider _clientProvider;
    public ClientService(IClientProvider clientProvider){
        _clientProvider = clientProvider;
    }

    public async Task<Client[]> GetClients(){
        List<Client> clients = _clientProvider.context;
        return await Task.FromResult(clients.ToArray());
    }

    public async Task<Client?> GetClientById(int id){
        Client? client = await 
            Task.FromResult(_clientProvider.context.FirstOrDefault(c => c.Id == id));
        return client;
    }

    public async Task<bool> ClientIsValid(Client client){
        // if client is not valid
        return false;

        // else return true
    }

    public void AddClient(Client client){
        _clientProvider.Add(client);
        _clientProvider.Save();
    }
}
