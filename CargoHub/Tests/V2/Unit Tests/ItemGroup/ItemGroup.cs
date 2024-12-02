using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text.Json;

namespace ItemGroupUnitTest
{
    [TestClass]
    public class ItemGroupUnitTest
    {
        [TestMethod]
        public void SerializeItemGroupToJson()
        {
            // Arrange
            var itemGroup = new ItemGroup
            {
                Id = 1,
                Name = "Fem Keijzer",
                Description = "",
                CreatedAt = "2019-09-22 15:51:07",
                UpdatedAt = "2022-05-18 13:49:28",
            };

            // Act
            string json = JsonSerializer.Serialize(itemGroup);

            // Assert
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void DeserializeJsonToItemGroup()
        {
            // Arrange
            string json = @"
            {
                ""id"": 1,
                ""name"": ""Fem Keijzer"",
                ""description"": """",
                ""created_at"": ""2019-09-22 15:51:07"",
                ""updated_at"": ""2022-05-18 13:49:28""
            }";

            // Act
            var itemGroup = JsonSerializer.Deserialize<ItemGroup>(json);

            // Assert
            Assert.IsNotNull(itemGroup);
            Assert.AreEqual(1, itemGroup.Id);
            Assert.AreEqual("Fem Keijzer", itemGroup.Name);
        }
    }
}