using apiV1.ValidationInterfaces;

namespace apiV1.Validations
{
    public class TransferValidationService : ITransferValidationService
    {
        private readonly ITransferProvider transferProvider;

        public TransferValidationService(ITransferProvider transferProvider)
        {
            this.transferProvider = transferProvider;
        }

        public bool IsTransferValid(Transfer? transfer, bool update = false)
        {
            if (transfer == null)
            {
                return false;
            }

            Transfer[] transfers = this.transferProvider.Get();
            bool transferExists = transfers.Any(t => t.Id == transfer.Id);
            if (update)
            {
                if (!transferExists)
                {
                    return false;
                }
            }
            else
            {
                if (transferExists)
                {
                    return false;
                }
            }

            if (transfer.Id < 0)
            {
                return false;
            }

            // if (string.IsNullOrWhiteSpace(transfer.Reference)) return false;
            if (transfer.TransferFrom < 0)
            {
                return false;
            }

            if (transfer.TransferTo < 0)
            {
                return false;
            }

            // if (string.IsNullOrWhiteSpace(transfer.TransferStatus)) return false;
            if (transfer.Items.Count == 0)
            {
                return false;
            }

            if (transfer.Items.Any(i => i.Amount < 0))
            {
                return false;
            }

            // if (transfer.Items.Any(i => string.IsNullOrWhiteSpace(i.ItemId))) return false;
            return true;
        }
    }
}