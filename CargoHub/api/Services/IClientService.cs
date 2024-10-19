public interface IClientService
{
    public Task<Client[]> GetClients();

    public Task<Client?> GetClientById(int id);

    public Task<bool> ClientIsValid(Client client);

    public Task AddClient(Client client);

    public Task<bool> UpdateClient(int id, Client updatedClient);

}
