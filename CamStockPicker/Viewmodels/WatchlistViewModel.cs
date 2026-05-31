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

    public WatchlistViewModel(WatchlistStore store)
    {
        Title = "Watchlist";
        _store = store;

        RefreshCommand = new Command(Load);
        Load();
    }

    private void Load()
    {
        Items.Clear();
        foreach (var item in _store.Load())
            Items.Add(item);
    }
}