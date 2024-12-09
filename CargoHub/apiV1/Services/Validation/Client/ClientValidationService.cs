using apiV1.ValidationInterfaces;

namespace apiV1.Validations
{
    public class ClientValidationService : IClientValidationService
    {
        private readonly IClientProvider clientProvider;

        public ClientValidationService(IClientProvider clientProvider)
        {
            this.clientProvider = clientProvider;
        }

        public async Task<bool> IsClientValidForPOST(Client newClient)
        {
            if (newClient == null)
            {
                return false;
            }

            if (newClient.Id < 0)
            {
                return false;
            }

            Client[] clients = this.clientProvider.Get();
            Client? client = await Task.FromResult(clients.FirstOrDefault(c => c.Id == newClient.Id));
            if (client != null)
            {
                return false;
            }

            // if (string.IsNullOrWhiteSpace(newClient.Name)) return false;
            // if (string.IsNullOrWhiteSpace(newClient.Address)) return false;
            // if (string.IsNullOrWhiteSpace(newClient.City)) return false;
            // if (string.IsNullOrWhiteSpace(newClient.ZipCode)) return false;
            // if (string.IsNullOrWhiteSpace(newClient.Province)) return false;
            // if (string.IsNullOrWhiteSpace(newClient.Country)) return false;
            // if (string.IsNullOrWhiteSpace(newClient.ContactName)) return false;
            // if (string.IsNullOrWhiteSpace(newClient.ContactPhone)) return false;
            // if (string.IsNullOrWhiteSpace(newClient.ContactEmail)) return false;
            return true;
        }

        public async Task<bool> IsClientValidForPUT(Client updatedClient, int clientId)
        {
            if (updatedClient == null)
            {
                return false;
            }

            if (updatedClient.Id < 0)
            {
                return false;
            }

            Client[] clients = this.clientProvider.Get();
            Client? client = await Task.FromResult(clients.FirstOrDefault(c => c.Id == updatedClient.Id));
            int index = clients.ToList().FindIndex(l => l.Id == clientId);
            if (index == -1)
            {
                return false;
            }

            if (client == null)
            {
                return false;
            }

            // if (string.IsNullOrWhiteSpace(updatedClient.Name)) return false;
            // if (string.IsNullOrWhiteSpace(updatedClient.Address)) return false;
            // if (string.IsNullOrWhiteSpace(updatedClient.City)) return false;
            // if (string.IsNullOrWhiteSpace(updatedClient.ZipCode)) return false;
            // if (string.IsNullOrWhiteSpace(updatedClient.Province)) return false;
            // if (string.IsNullOrWhiteSpace(updatedClient.Country)) return false;
            // if (string.IsNullOrWhiteSpace(updatedClient.ContactName)) return false;
            // if (string.IsNullOrWhiteSpace(updatedClient.ContactPhone)) return false;
            // if (string.IsNullOrWhiteSpace(updatedClient.ContactEmail)) return false;
            return true;
        }
    }
}