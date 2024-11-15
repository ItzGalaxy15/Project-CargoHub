using apiV2.ValidationInterfaces;

namespace apiV2.Validations
{
    public class ClientValidationService : IClientValidationService
    {
        private readonly IClientProvider _clientProvider;
        public ClientValidationService(IClientProvider clientProvider){
            _clientProvider = clientProvider;
        }

        public async Task<bool> IsClientValidForPOST(Client newClient){
            if (newClient == null) return false;
            if (newClient.Id < 0) return false;
            Client[] clients = _clientProvider.Get();
            Client? client = await Task.FromResult(clients.FirstOrDefault(c => c.Id == newClient.Id));
            if (client != null) return false;
            return true;
        }

        public async Task<bool> IsClientValidForPUT(Client updatedClient, int clientId)
        {
            if (updatedClient == null) return false;
            if (updatedClient.Id < 0) return false;
            Client[] clients = _clientProvider.Get();
            Client? client = await Task.FromResult(clients.FirstOrDefault(c => c.Id == updatedClient.Id));
            int index = clients.ToList().FindIndex(l => l.Id == clientId);
            if (index == -1) return false;
            if (client == null) return false;
            return true;
        }
        public async Task<bool> IsClientValidForPATCH(Dictionary<string, dynamic> patch, int clientId){
            if (patch == null) return false;
            var validProperties = new HashSet<string> {
                "name",
                "address",
                "city",
                "zip_code",
                "province",
                "country",
                "contact_name",
                "contact_phone",
                "contact_email"
            };
            Client[] clients = _clientProvider.Get();
            Client? client = await Task.FromResult(clients.FirstOrDefault(c => c.Id == clientId));

            if (client == null) return false;

            var validKeysInPatch = new List<string>();
            foreach (var key in patch.Keys)
            {
                if (validProperties.Contains(key))
                {
                    validKeysInPatch.Add(key);
                }
            }

            if (!validKeysInPatch.Any())
            {
                return false;
            }
            return true;
        }
    }
}