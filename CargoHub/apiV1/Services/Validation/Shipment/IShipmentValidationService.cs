namespace apiV1.ValidationInterfaces
{
    public interface IShipmentValidationService
    {
        public bool IsShipmentValid(Shipment? shipment, bool update = false);
    }
}