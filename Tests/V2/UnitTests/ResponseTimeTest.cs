using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


[TestClass]
public class ApiResponseTimeTests
{
    private static HttpClient _client;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        // Initialize HttpClient with the base address of your running API
        _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:3000") // Adjust the base address to match your API
        };
    }

    [DataTestMethod]
    [DataRow("/api/v2/clients")]
    [DataRow("/api/v2/inventories")]
    [DataRow("/api/v2/item_lines")]
    [DataRow("/api/v2/items")]
    [DataRow("/api/v2/item_groups")]
    [DataRow("/api/v2/item_types")]
    [DataRow("/api/v2/locations")]
    [DataRow("/api/v2/orders")]
    [DataRow("/api/v2/shipments")]
    [DataRow("/api/v2/suppliers")]
    [DataRow("/api/v2/transfers")]
    [DataRow("/api/v2/warehouses")]
    public async Task ApiResponse_Should_CompleteWithin500ms(string endpoint)
    {
        var stopwatch = Stopwatch.StartNew();

        var request = new HttpRequestMessage(HttpMethod.Get, endpoint); // Use the parameterized endpoint
        request.Headers.Add("API_KEY", "a1b2c3d4e5"); // Replace with your actual API key

        var response = await _client.SendAsync(request);

        stopwatch.Stop();

        Assert.IsTrue(stopwatch.ElapsedMilliseconds <= 500, $"API response time for {endpoint} exceeded 500ms");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}