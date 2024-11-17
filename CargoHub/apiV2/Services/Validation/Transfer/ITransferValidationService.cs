namespace apiV2.ValidationInterfaces
{
    public interface ITransferValidationService
    {
        public bool IsTransferValid(Transfer? transfer, bool update = false);

        public Task<bool> IsTransferValidForPATCH(Dictionary<string, dynamic> patch, int transferId);
    }
}
