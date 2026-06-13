using System.Collections.ObjectModel;
using System.Windows.Input;
using CamStockPicker.Models;
using CamStockPicker.Services;

namespace CamStockPicker.ViewModels;

/// <summary>
/// Controls the localised data bindings for favoured assets.
/// </summary>
public class WatchlistViewModel : BaseViewModel
{
    private readonly WatchlistStore _store;

    public ObservableCollection<WatchlistItem> Items { get; } = new();

    public ICommand RefreshCommand { get; }
    public ICommand OpenDetailCommand { get; }

    public WatchlistViewModel(WatchlistStore store)
    {
        Title = "Watchlist";
        _store = store;

        RefreshCommand = new Command(Load);
        OpenDetailCommand = new Command<WatchlistItem>(async item => await OpenDetailAsync(item));

        Load();
    }

    private void Load()
    {
        Items.Clear();
        foreach (var item in _store.Load())
            Items.Add(item);
    }

    private async Task OpenDetailAsync(WatchlistItem? item)
    {
        if (item is null) return;

        await Shell.Current.GoToAsync(
            $"detail?symbol={Uri.EscapeDataString(item.Symbol)}&name={Uri.EscapeDataString(item.Name)}");
    }
}