using System.Text.Json.Serialization;

namespace CamStockPicker.Models
{
    /// <summary>
    /// Represents an individual stock match found during a search query.
    /// </summary>
    public class StockSearchResult
    {
        [JsonPropertyName("1. symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("2. name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("3. type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("4. region")]
        public string Region { get; set; } = string.Empty;

        [JsonPropertyName("8. currency")]
        public string Currency { get; set; } = string.Empty;
    }
}