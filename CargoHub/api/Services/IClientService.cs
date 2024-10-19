public interface IClientService
{
    public Task<Client[]> GetClients();

    public Task<Client?> GetClientById(int id);

    public Task<bool> ClientIsValid(Client client);

    public Task<bool> AddClient(Client client);

}
