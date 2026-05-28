using System.Text.Json.Serialization;

namespace CamStockPicker.Models
{
    /// <summary>
    /// Container that holds a stock quote response.
    /// <summary>
    public class GlobalQuoteResponse
    {
        [JsonPropertyName("Global Quote")]
        public StockQuote? Quote { get; set; }
    }
}