namespace apiV2.ValidationInterfaces
{
    public interface IShipmentValidationService
    {
        public bool IsShipmentValid(Shipment? shipment, bool update = false);
        public bool isItemSmallValid(ItemSmall[] item);
        public Task<bool> IsShipmentValidForPATCH(Dictionary<string, dynamic> patch, int shipmentId);
        public bool IsShipmentCommitValid(Shipment shipment);
    }
}