using System.Collections.ObjectModel;
using System.Windows.Input;
using CamStockPicker.Models;
using CamStockPicker.Services;

namespace CamStockPicker.ViewModels;

public class SearchViewModel : BaseViewModel
{
    private readonly AlphaAPI _api;

    private string query = string.Empty;

    public string Query
    {
        get => query;
        set => SetProperty(ref query, value);
    }

    public ObservableCollection<StockSearchResult> Results { get; } = new();

    public ICommand SearchCommand { get; }
    public ICommand OpenDetailCommand { get; }

    public SearchViewModel(AlphaAPI api)
    {
        Title = "Search";
        _api = api;

        SearchCommand = new Command(async () => await SearchAsync(), () => !IsBusy);
        OpenDetailCommand = new Command<StockSearchResult>(async item => await OpenDetailAsync(item));
    }

    private async Task SearchAsync()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Query))
        {
            Results.Clear();
            return;
        }

        try
        {
            IsBusy = true;
            Results.Clear();

            var matches = await _api.SearchStocksAsync(Query);
            foreach (var m in matches)
                Results.Add(m);
        }
        finally
        {
            IsBusy = false;
            (SearchCommand as Command)?.ChangeCanExecute();
        }
    }

    private async Task OpenDetailAsync(StockSearchResult? item)
    {
        if (item is null) return;

        // pass symbol + name to detail via query string
        await Shell.Current.GoToAsync(
            $"detail?symbol={Uri.EscapeDataString(item.Symbol)}&name={Uri.EscapeDataString(item.Name)}");
    }
}