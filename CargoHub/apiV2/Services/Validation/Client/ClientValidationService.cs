using System.Text.Json;
using apiV2.ValidationInterfaces;

namespace apiV2.Validations
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

            return true;
        }

        public async Task<bool> IsClientValidForPATCH(Dictionary<string, dynamic> patch, int clientId)
        {
            if (patch == null)
            {
                return false;
            }

            var validProperties = new Dictionary<string, JsonValueKind>
            {
                { "name", JsonValueKind.String },
                { "address", JsonValueKind.String },
                { "city", JsonValueKind.String },
                { "zip_code", JsonValueKind.String },
                { "province", JsonValueKind.String },
                { "country", JsonValueKind.String },
                { "contact_name", JsonValueKind.String },
                { "contact_phone", JsonValueKind.String },
                { "contact_email", JsonValueKind.String },
            };
            Client[] clients = this.clientProvider.Get();
            Client? client = await Task.FromResult(clients.FirstOrDefault(c => c.Id == clientId));

            if (client == null)
            {
                return false;
            }

            var validKeysInPatch = new List<string>();
            foreach (var key in patch.Keys)
            {
                if (validProperties.ContainsKey(key))
                {
                    var expectedType = validProperties[key];
                    JsonElement value = patch[key];
                    if (value.ValueKind != expectedType)
                    {
                        patch.Remove(key);

                        // remove key if not valid type
                    }
                    else
                    {
                        validKeysInPatch.Add(key);
                    }
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