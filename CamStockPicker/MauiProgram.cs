using CamStockPicker.Services;
using CamStockPicker.ViewModels;
using CamStockPicker.Views;
using Microsoft.Extensions.Logging;

namespace CamStockPicker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Services / Stores
        builder.Services.AddSingleton<AlphaAPI>();
        builder.Services.AddSingleton<WatchlistStore>();
        builder.Services.AddSingleton<AppSettingsStore>();

        // ViewModels
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<DetailViewModel>();
        builder.Services.AddTransient<WatchlistViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        // Views
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<WatchlistPage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder.Build();
    }
}