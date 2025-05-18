using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MizeProject.Utils;

namespace MizeProject.Storages
{
    public class WebServiceStorage<T> : IStorage<T>
    {
        private readonly string _apiUrl;
        private static readonly ILogger _logger = LoggerManager.CreateLogger<WebServiceStorage<T>>();


        public bool CanWrite => false;

        public TimeSpan ExpirationTime => TimeSpan.Zero;

        public WebServiceStorage(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<(T value, DateTime FetchTime)> ReadAsync()
        {
            using var client = new HttpClient();

            try
            {
                _logger.LogInformation("Reading from API web service");
                var response = await client.GetAsync(_apiUrl);
                response.EnsureSuccessStatusCode();

                var responseAsJsonStr = await response.Content.ReadAsStringAsync();
                var value = JsonSerializer.Deserialize<T>(responseAsJsonStr);

                return (value, DateTime.UtcNow);
            }
            catch
            {
                _logger.LogError("Failed to read from API web service");
                return (default(T), DateTime.MinValue);
            }
        }

        public Task WriteAsync(T value)
        {
            throw new NotSupportedException("WebServiceStorage is read-only, no Write action available!");
        }
    }
}
