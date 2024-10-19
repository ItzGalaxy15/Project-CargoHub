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

}
