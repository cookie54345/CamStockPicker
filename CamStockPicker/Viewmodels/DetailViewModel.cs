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
                _ = LoadAsync(); // auto-load when navigated to
        }
    }

    public string CompanyName
    {
        get => companyName;
        set => SetProperty(ref companyName, value);
    }

    public string PriceText => quote is null ? "--" : ToMoney(quote.Price);
    public string OpenText => quote is null ? "--" : ToMoney(quote.Open);
    public string HighText => quote is null ? "--" : ToMoney(quote.High);
    public string LowText => quote is null ? "--" : ToMoney(quote.Low);
    public string PrevCloseText => quote is null ? "--" : ToMoney(quote.PreviousClose);

    public string ChangeText
    {
        get
        {
            if (quote is null) return string.Empty;

            var change = quote.Change;
            var changePercent = quote.ChangePercent; // often "5.23%" in AlphaVantage payloads

            var mode = _settings.GetChangeDisplayMode();
            if (mode == ChangeDisplayMode.Percent)
                return changePercent;

            // Dollars mode
            var sign = change >= 0 ? "+" : "-";
            return $"{sign}{Math.Abs(change):0.##}";
        }
    }

    public Color ChangeColor
    {
        get
        {
            if (quote is null) return Colors.Transparent;
            return quote.Change >= 0 ? Colors.Green : Colors.Red;
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

            // notify computed properties
            OnPropertyChanged(nameof(PriceText));
            OnPropertyChanged(nameof(OpenText));
            OnPropertyChanged(nameof(HighText));
            OnPropertyChanged(nameof(LowText));
            OnPropertyChanged(nameof(PrevCloseText));
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