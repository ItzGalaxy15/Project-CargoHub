public interface IClientService
{
    public Task<Client[]> GetClients();

    public Task<Client?> GetClientById(int id);

}
