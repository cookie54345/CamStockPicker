using System.Text.Json.Serialization;

namespace CamStockPicker.Models
{
    /// <summary>
    /// Holds the actual price data for a specific stock (Alpha Vantage GLOBAL_QUOTE).
    /// </summary>
    public class StockQuote
    {
        [JsonPropertyName("01. symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("02. open")]
        public string Open { get; set; } = string.Empty;

        [JsonPropertyName("03. high")]
        public string High { get; set; } = string.Empty;

        [JsonPropertyName("04. low")]
        public string Low { get; set; } = string.Empty;

        [JsonPropertyName("05. price")]
        public string Price { get; set; } = string.Empty;

        [JsonPropertyName("08. previous close")]
        public string PreviousClose { get; set; } = string.Empty;

        [JsonPropertyName("10. change percent")]
        public string ChangePercent { get; set; } = string.Empty;
    }
}