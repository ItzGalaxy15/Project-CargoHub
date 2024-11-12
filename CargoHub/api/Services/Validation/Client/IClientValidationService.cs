namespace api.ValidationInterface
{
    public interface IClientValidationService
    {
        public Task<bool> IsClientValidForPOST(Client Client);
        public Task<bool> IsClientValidForPUT(Client Client, int clientId);
    }
}