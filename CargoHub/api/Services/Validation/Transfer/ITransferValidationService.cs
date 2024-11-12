public interface ITransferValidationService
{
    public bool IsTransferValid(Transfer? transfer, bool update = false);
}