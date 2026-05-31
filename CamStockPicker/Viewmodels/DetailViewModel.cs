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

    // Statistics texts (now backed by StockQuote fields)
    public string OpenText => quote is null ? "--" : ToMoney(quote.Open);
    public string HighText => quote is null ? "--" : ToMoney(quote.High);
    public string LowText => quote is null ? "--" : ToMoney(quote.Low);
    public string PrevCloseText => quote is null ? "--" : ToMoney(quote.PreviousClose);

    public string ChangeText
    {
        get
        {
            if (quote is null) return string.Empty;

            var mode = _settings.GetChangeDisplayMode();

            // quote.ChangePercent looks like "1.23%" or "-0.83%"
            var pct = TryParsePercent(quote.ChangePercent); // decimal fraction, e.g. 0.0123

            if (mode == ChangeDisplayMode.Percent)
            {
                // Keep it consistent even if API formatting varies
                return pct is null ? quote.ChangePercent : $"{pct.Value:+0.##%;-0.##%;0%}";
            }

            // Dollars mode: approximate $ change from Price * percent
            if (pct is null) return string.Empty;

            if (!double.TryParse(quote.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                return string.Empty;

            var change = price * pct.Value;
            return change.ToString("+#,##0.00;-#,##0.00;0.00", CultureInfo.InvariantCulture);
        }
    }

    public Color ChangeColor
    {
        get
        {
            if (quote is null) return Colors.Transparent;

            var pct = TryParsePercent(quote.ChangePercent);
            if (pct is null)
                return quote.ChangePercent.TrimStart().StartsWith("-") ? Colors.Red : Colors.Green;

            return pct.Value < 0 ? Colors.Red : Colors.Green;
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

            // statistics
            OnPropertyChanged(nameof(OpenText));
            OnPropertyChanged(nameof(HighText));
            OnPropertyChanged(nameof(LowText));
            OnPropertyChanged(nameof(PrevCloseText));
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

    private static double? TryParsePercent(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;

        // "1.23%" -> 0.0123
        var cleaned = text.Trim().Replace("%", "");
        if (!double.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var pct))
            return null;

        return pct / 100.0;
    }
}