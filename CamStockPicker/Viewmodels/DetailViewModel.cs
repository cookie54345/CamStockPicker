using System.Globalization;
using System.Windows.Input;
using CamStockPicker.Models;
using CamStockPicker.Services;

namespace CamStockPicker.ViewModels;

/// <summary>
/// Coordinates the data logic for a targeted stock asset.
/// </summary>

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

            var pct = TryParsePercent(quote.ChangePercent); 

            if (mode == ChangeDisplayMode.Percent)
            {
                return pct is null ? quote.ChangePercent : $"{pct.Value:+0.##%;-0.##%;0%}";
            }

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

        var cleaned = text.Trim().Replace("%", "");
        if (!double.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var pct))
            return null;

        return pct / 100.0;
    }
}