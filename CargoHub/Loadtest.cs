using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Loadtest
{
    public class Loadtest2
    {
        public static async Task Main()
        {
            // Loadtest for x amount concurrent users
            int concurrentUsers = 15000;
            string apiUrl = "http://localhost:3000/api/v2/clients";

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
                request.Headers.Add("API_KEY", "a1b2c3d4e5");

                var response = await client.SendAsync(request);
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