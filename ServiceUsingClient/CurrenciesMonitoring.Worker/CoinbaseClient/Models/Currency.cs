using System.Text.Json.Serialization;

#nullable disable

namespace CurrenciesMonitoring.Worker.CoinbaseClient.Models
{
    public class Currency
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("min_size")]
        public string MinSize { get; set; }
    }
}