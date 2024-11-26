namespace apiV2.Interfaces
{    
    public interface ITransferService
    {    
        public Task AddTransfer(Transfer transfer);

        public Transfer[] GetTransfers();

        public Transfer? GetTransferById(int id);

        public ItemSmall[] GetItemsByTransferId(int transferId);
        
        public Task<bool> UpdateTransfer(Transfer transfer, int transferId);

        public Task DeleteTransfer(Transfer transfer);

        public Task PatchTransfer(int id, Dictionary<string, dynamic> patch, Transfer transfer); 
    }}