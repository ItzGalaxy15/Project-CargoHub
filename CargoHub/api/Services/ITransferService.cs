public interface ITransferService
{
    public Transfer[] GetTransfers();

    public Transfer? GetTransferById(int id);
}