using System.Text.Json.Serialization;

public class Shipment : Base
{
    [JsonIgnore]
    public override string _path { get; set; } = "shipments.json";


    [JsonPropertyName("id")]
    public required int Id { get; set; }
    
    [JsonPropertyName("order_id")]
    public required int OrderId { get; set; }

    [JsonPropertyName("source_id")]
    public required int SourceId { get; set; }

    [JsonPropertyName("order_date")]
    public required string OrderDate { get; set; }

    [JsonPropertyName("request_date")]
    public required string RequestDate { get; set; }

    [JsonPropertyName("shipment_date")]
    public required string ShipmentDate { get; set; }

    [JsonPropertyName("shipment_type")]
    public required string ShipmentType { get; set; }

    [JsonPropertyName("shipment_status")]
    public required string ShipmentStatus { get; set; }

    [JsonPropertyName("notes")]
    public required string Notes { get; set; }

    [JsonPropertyName("carrier_code")]
    public required string CarrierCode { get; set; }

    [JsonPropertyName("carrier_description")]
    public required string CarrierDescription { get; set; }

    [JsonPropertyName("service_code")]
    public required string ServiceCode { get; set; }

    [JsonPropertyName("payment_type")]
    public required string PaymentType { get; set; }
    
    [JsonPropertyName("transfer_mode")]
    public required string TransferMode { get; set; }

    [JsonPropertyName("total_package_count")]
    public required int TotalPackageCount { get; set; }

    [JsonPropertyName("total_package_weight")]
    public required int TotalPackageWeight { get; set; }

    [JsonPropertyName("items")]
    public required List<ItemSmall> Items { get; set; }

}
