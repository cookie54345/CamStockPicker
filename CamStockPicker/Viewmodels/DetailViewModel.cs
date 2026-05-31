using System.Globalization;
using System.Windows.Input;
using CamStockPicker.Models;
using CamStockPicker.Services;

namespace CamStockPicker.ViewModels;

[QueryProperty(nameof(Symbol), "symbol")]
[QueryProperty(nameof(CompanyName), "name")]
public class DetailViewModel : BaseViewModel
{
    private readonly AlphaAPI _api;
    private readonly WatchlistStore _watchlist;
    private readonly AppSettingsStore _settings;

    private string symbol = string.Empty;
    private string companyName = string.Empty;

    private StockQuote? quote;

    public string Symbol
    {
        get => symbol;
        set
        {
            if (SetProperty(ref symbol, value))
                _ = LoadAsync();
        }
    }

    public string CompanyName
    {
        get => companyName;
        set => SetProperty(ref companyName, value);
    }

    public string PriceText => quote is null ? "--" : ToMoney(quote.Price);

    public string ChangeText
    {
        get
        {
            if (quote is null) return string.Empty;

            // Your API model currently only provides ChangePercent (e.g. "5.23%")
            // If user selects Dollars mode, we’ll still show percent until you expand the model.
            var mode = _settings.GetChangeDisplayMode();
            return mode == ChangeDisplayMode.Percent ? quote.ChangePercent : quote.ChangePercent;
        }
    }

    public Color ChangeColor
    {
        get
        {
            if (quote is null) return Colors.Transparent;

            // Infer sign from the string (simple but works)
            // e.g. "-1.23%" -> Red, "1.23%" -> Green
            return quote.ChangePercent.TrimStart().StartsWith("-") ? Colors.Red : Colors.Green;
        }
    }

    public ICommand RefreshCommand { get; }
    public ICommand AddToWatchlistCommand { get; }

    public DetailViewModel(AlphaAPI api, WatchlistStore watchlist, AppSettingsStore settings)
    {
        Title = "Detail";
        _api = api;
        _watchlist = watchlist;
        _settings = settings;

        RefreshCommand = new Command(async () => await LoadAsync());
        AddToWatchlistCommand = new Command(AddToWatchlist);
    }

    private async Task LoadAsync()
    {
        if (IsBusy) return;
        if (string.IsNullOrWhiteSpace(Symbol)) return;

        try
        {
            IsBusy = true;
            quote = await _api.GetQuoteAsync(Symbol);

            OnPropertyChanged(nameof(PriceText));
            OnPropertyChanged(nameof(ChangeText));
            OnPropertyChanged(nameof(ChangeColor));
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void AddToWatchlist()
    {
        if (string.IsNullOrWhiteSpace(Symbol)) return;

        _watchlist.AddOrUpdate(new WatchlistItem
        {
            Symbol = Symbol,
            Name = CompanyName
        });
    }

    private static string ToMoney(string? value)
    {
        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
            return d.ToString("C2", CultureInfo.GetCultureInfo("en-US"));

        return "--";
    }
}