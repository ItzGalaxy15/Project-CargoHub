public class ClientService : IClientService
{
    private readonly IClientProvider _clientProvider;
    public ClientService(IClientProvider clientProvider){
        _clientProvider = clientProvider;
    }

    public async Task<Client[]> GetClients(){
        Client[] clients = _clientProvider.Get();
        return await Task.FromResult(clients.ToArray());
    }

    public async Task<Client?> GetClientById(int id){
        Client[] clients = _clientProvider.Get();
        Client? client = await 
            Task.FromResult(clients.FirstOrDefault(c => c.Id == id));
        return client;
    }
    public async Task<bool> ClientIsValid(Client newClient){
        if (newClient == null) return false;
        if (newClient.Id <= 0) return false;
        Client[] clients = _clientProvider.Get();
        Client? client = await Task.FromResult(clients.FirstOrDefault(c => c.Id == newClient.Id));
        if (client != null) return false;
        return true;
    }

    public async Task<bool> AddClient(Client client){
        client.CreatedAt = client.GetTimeStamp();
        client.UpdatedAt = client.GetTimeStamp();
        _clientProvider.Add(client);
        await _clientProvider.Save();
        return true;
    }

}
