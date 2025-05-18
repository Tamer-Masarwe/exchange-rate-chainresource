using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MizeProject.Models
{
    public class ExchangeRateModel
    {
        [JsonPropertyName("disclaimer")]
        public string Disclaimer { get; set; }

        [JsonPropertyName("license")]
        public string License { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("base")]
        public string BaseCurrency { get; set; } 

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
