using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Loadtest
{
    public class Loadtest2
    {
        public static async Task Main(string[] args)
        {
            int concurrentUsers = 15000;
            string apiUrl = "http://localhost:3000/api/v2/clients"; // Replace with your API endpoint

            var tasks = new Task[concurrentUsers];
            for (int i = 0; i < concurrentUsers; i++)
            {
                tasks[i] = SimulateUser(apiUrl);
                Console.WriteLine("testing user: " + i);
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All requests completed.");
        }

        private static async Task SimulateUser(string apiUrl)
        {
            using var client = new HttpClient();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.Add("API_KEY", "a1b2c3d4e5"); // Replace with your actual API key

                var response = await client.SendAsync(request); // Adjust HTTP method as needed
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}