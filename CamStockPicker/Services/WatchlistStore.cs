using System.Text.Json;
using CamStockPicker.Models;

namespace CamStockPicker.Services;

/// <summary>
/// Handles persistent storage routines for saved stock favorites.
/// </summary>

public class WatchlistStore
{
    private const string Key = "watchlist_v1";

    public List<WatchlistItem> Load()
    {
        var json = Preferences.Get(Key, string.Empty);
        if (string.IsNullOrWhiteSpace(json))
            return new List<WatchlistItem>();

        return JsonSerializer.Deserialize<List<WatchlistItem>>(json) ?? new List<WatchlistItem>();
    }

    public void Save(List<WatchlistItem> items)
    {
        Preferences.Set(Key, JsonSerializer.Serialize(items));
    }

    public void AddOrUpdate(WatchlistItem item)
    {
        var items = Load();
        var existing = items.FirstOrDefault(x => x.Symbol.Equals(item.Symbol, StringComparison.OrdinalIgnoreCase));

        if (existing is null)
            items.Add(item);
        else
            existing.Name = item.Name;

        Save(items);
    }

    public void Remove(string symbol)
    {
        var items = Load();
        items.RemoveAll(x => x.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        Save(items);
    }
}