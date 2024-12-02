using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace WarehouseUnitTest
{

    [TestClass]
    public class WarehouseUnitTest
    {
        [TestMethod]
        public void SerializeWarehouseToJson()
        {
            // Arrange
            var contact = new WarehouseContact
            {
                Name = "Fem Keijzer",
                Phone = "(078) 0013363",
                Email = "blamore@example.net"
            };

            var warehouse = new Warehouse
            {
                Id = 1,
                Code = "YQZZNL56",
                Name = "Heemskerk cargo hub",
                Address = "Karlijndreef 281",
                Zip = "4002 AS",
                City = "City",
                Province = "Friesland",
                Country = "NL",
                Contact = contact,
                CreatedAt = "1983-04-13 04:59:55",
                UpdatedAt = "2007-02-08 20:11:00"
            };

            // Act
            string json = JsonSerializer.Serialize(warehouse);

            // Assert
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void DeserializeJsonToWarehouse()
        {
            // Arrange
            string json = @"
            {
                ""id"": 1,
                ""code"": ""YQZZNL56"",
                ""name"": ""Heemskerk cargo hub"",
                ""address"": ""Karlijndreef 281"",
                ""zip"": ""4002 AS"",
                ""city"": ""City"",
                ""province"": ""Friesland"",
                ""country"": ""NL"",
                ""contact"": {
                    ""name"": ""Fem Keijzer"",
                    ""phone"": ""(078) 0013363"",
                    ""email"": ""blamore@example.net""
                },
                ""created_at"": ""1983-04-13 04:59:55"",
                ""updated_at"": ""2007-02-08 20:11:00""
            }";

            // Act
            var warehouse = JsonSerializer.Deserialize<Warehouse>(json);

            // Assert
            Assert.IsNotNull(warehouse);
            Assert.AreEqual(1, warehouse.Id);
            Assert.AreEqual("YQZZNL56", warehouse.Code);
            Assert.AreEqual("Fem Keijzer", warehouse.Contact.Name);
        }
    }
}