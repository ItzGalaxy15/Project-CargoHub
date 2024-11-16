namespace apiV2.ValidationInterfaces
{
    public interface IShipmentValidationService
    {
        public bool IsShipmentValid(Shipment? shipment, bool update = false);

        public Task<bool> IsShipmentValidForPATCH(Dictionary<string, dynamic> patch, int shipmentId);
    }
}