namespace apiV2.ValidationInterfaces
{
    public interface IClientValidationService
    {
        public Task<bool> IsClientValidForPOST(Client client);

        public Task<bool> IsClientValidForPUT(Client client, int clientId);

        public Task<bool> IsClientValidForPATCH(Dictionary<string, dynamic> patch, int clientId);
    }
}