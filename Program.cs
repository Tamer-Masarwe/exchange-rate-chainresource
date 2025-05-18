

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MizeProject.Core;
using MizeProject.Models;
using MizeProject.Storages;
using MizeProject.Utils;

namespace MizeProject
{
    public class Program
    {
        private static readonly ILogger _logger = LoggerManager.CreateLogger<Program>();

        public static async Task Main(string[] args)
        {
            _logger.LogInformation("Application started.");

            try
            {
                //Load api keys
                var config = new ConfigurationBuilder()
                    .AddJsonFile("Config/appsettings.json")
                    .Build();

                var appId = config["APIKeys:AppId"];
                var baseUrl = config["APIKeys:BaseUrl"];
                var apiUrl = $"{baseUrl}?app_id={appId}";

                if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(baseUrl))
                {
                    _logger.LogError("Missing AppId or BaseUrl in appsettings configuration file");
                    return;
                }

                const string exchangeRatesJsonFile = "exchangeRates.json";

                // Setup storages and Create chain resource 
                var chain = new ChainResource<ExchangeRateModel>(
                    [
                        new MemoryStorage<ExchangeRateModel>(TimeSpan.FromHours(1)),//For memory
                    new FileSystemStorage<ExchangeRateModel>(exchangeRatesJsonFile, TimeSpan.FromHours(4)),//For file system
                    new WebServiceStorage<ExchangeRateModel>(apiUrl)//For API web service
                    ]
                );

                // Fetch the data
                var exchangeRates = await chain.GetValue();

                //Print result
                if (exchangeRates != null)
                {
                    Console.WriteLine($"Base Currency: {exchangeRates.BaseCurrency}");
                    Console.WriteLine($"Timestamp: {exchangeRates.Timestamp}");
                    Console.WriteLine("Top 5 Rates:");
                    foreach (var kv in exchangeRates.Rates.Take(5))
                    {
                        Console.WriteLine($"  {kv.Key}: {kv.Value}");
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to retrieve exchange rates from any source");
                    Console.WriteLine("Failed to retrieve exchange rates");
                }

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Unhandled exception in Main method.");
                Console.WriteLine("An unexpected error occurred. See attached logs for more details");
            }

            _logger.LogInformation("Application finished");
        }
    }
}