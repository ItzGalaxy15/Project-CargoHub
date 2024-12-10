using apiV1.Interfaces;

namespace apiV1.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientProvider clientProvider;

        public ClientService(IClientProvider clientProvider)
        {
            this.clientProvider = clientProvider;
        }

        public async Task<Client[]> GetClients()
        {
            Client[] clients = this.clientProvider.Get();
            return await Task.FromResult(clients.ToArray());
        }

        public async Task<Client?> GetClientById(int id)
        {
            Client[] clients = this.clientProvider.Get();
            Client? client = await
                Task.FromResult(clients.FirstOrDefault(c => c.Id == id));
            return client;
        }

        public async Task AddClient(Client client)
        {
            client.CreatedAt = client.GetTimeStamp();
            client.UpdatedAt = client.GetTimeStamp();
            this.clientProvider.Add(client);
            await this.clientProvider.Save();
        }

        public async Task UpdateClient(int id, Client updatedClient)
        {
            updatedClient.UpdatedAt = updatedClient.GetTimeStamp();
            this.clientProvider.Update(updatedClient, id);
            await this.clientProvider.Save();
        }

        public async Task DeleteClient(Client client)
        {
            this.clientProvider.Delete(client);
            await this.clientProvider.Save();
        }
    }
}