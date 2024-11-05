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

    public async Task AddClient(Client client){
        client.CreatedAt = client.GetTimeStamp();
        client.UpdatedAt = client.GetTimeStamp();
        _clientProvider.Add(client);
        await _clientProvider.Save();
    }

    public async Task UpdateClient(int id, Client updatedClient){
        updatedClient.UpdatedAt = updatedClient.GetTimeStamp();
        _clientProvider.Update(updatedClient, id);
        await _clientProvider.Save();
    }

    public async Task DeleteClient(Client client){
        _clientProvider.Delete(client);
        await _clientProvider.Save();
    }
}
