namespace apiV1.ValidationInterfaces
{
    public interface IClientValidationService
    {
        public Task<bool> IsClientValidForPOST(Client client);

        public Task<bool> IsClientValidForPUT(Client client, int clientId);
    }
}