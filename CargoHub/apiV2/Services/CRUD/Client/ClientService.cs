using System.Text.Json;
using apiV2.Interfaces;

namespace apiV2.Services
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

        public async Task PatchClient(int id, Dictionary<string, dynamic> patch, Client client)
        {
            foreach (var key in patch.Keys)
            {
                var value = patch[key];
                if (value is JsonElement jsonElement)
                {
                    switch (key)
                    {
                        case "name":
                            client.Name = jsonElement.GetString()!;
                            break;
                        case "address":
                            client.Address = jsonElement.GetString()!;
                            break;
                        case "city":
                            client.City = jsonElement.GetString()!;
                            break;
                        case "zip_code":
                            client.ZipCode = jsonElement.GetString()!;
                            break;
                        case "province":
                            client.Province = jsonElement.GetString()!;
                            break;
                        case "country":
                            client.Country = jsonElement.GetString()!;
                            break;
                        case "contact_name":
                            client.ContactName = jsonElement.GetString()!;
                            break;
                        case "contact_phone":
                            client.ContactPhone = jsonElement.GetString()!;
                            break;
                        case "contact_email":
                            client.ContactEmail = jsonElement.GetString()!;
                            break;
                    }
                }
            }

            client.UpdatedAt = client.GetTimeStamp();
            this.clientProvider.Update(client, id);
            await this.clientProvider.Save();
        }
    }
}