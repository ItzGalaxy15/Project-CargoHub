namespace apiV2.Interfaces
{
    public interface IClientService
    {
        public Task<Client[]> GetClients();

        public Task<Client?> GetClientById(int id);

        public Task AddClient(Client client);

        public Task UpdateClient(int id, Client updatedClient);

        public Task DeleteClient(Client client);

        public Task PatchClient(int id, Dictionary<string, object> patch, Client client);
    }
}
