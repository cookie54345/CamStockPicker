using System.Text.Json.Serialization;

namespace CamStockPicker.Models
{
    /// <summary>
    /// The container for search results.
    /// <summary>
    public class StockSearchResponse
    {
        [JsonPropertyName("bestMatches")]
        public List<StockSearchResult> BestMatches { get; set; } = new();
    }
}