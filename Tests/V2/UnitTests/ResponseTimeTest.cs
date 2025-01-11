using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CargoHub.Tests
{
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

        [TestMethod]
        public async Task ApiResponse_Should_CompleteWithin500ms()
        {
            var stopwatch = Stopwatch.StartNew();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v2/clients"); // Adjust the endpoint to match your API
            request.Headers.Add("API_KEY", "a1b2c3d4e5"); // Replace with your actual API key

            var response = await _client.SendAsync(request);

            stopwatch.Stop();

            Assert.IsTrue(stopwatch.ElapsedMilliseconds <= 500, "API response time exceeded 500ms");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}