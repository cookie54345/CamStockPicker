using System.Text.Json.Serialization;

namespace CamStockPicker.Models
{
    /// <summary>
    /// Represents an individual stock match found during a search query.
    /// </summary>
    public class StockSearchResult
    {
        [JsonPropertyName("01. symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("02. name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("03. type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("04. region")]
        public string Region { get; set; } = string.Empty;

        [JsonPropertyName("08. currency")]
        public string Currency { get; set; } = string.Empty;
    }
}