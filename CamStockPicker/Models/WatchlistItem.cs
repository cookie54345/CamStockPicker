namespace CamStockPicker.Models;

public class WatchlistItem
{
    /// <summary>
    /// Represents a stock profile saved to the watchlist.
    /// </summary>
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}