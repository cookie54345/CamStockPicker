using System.Text.Json.Serialization;

namespace CamStockPicker.Models
{
    /// <summary>
    /// Holds the actual price data for a specific stock.
    /// <summary>
    public class StockQuote
    {
        [JsonPropertyName("01. symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("05. price")]
        public string Price { get; set; } = string.Empty;

        [JsonPropertyName("10. change percent")]
        public string ChangePercent { get; set; } = string.Empty;
    }
}