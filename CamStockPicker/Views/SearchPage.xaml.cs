using CamStockPicker.ViewModels;
#if ANDROID
using Android.Content.PM;
using Microsoft.Maui.ApplicationModel;
#endif

namespace CamStockPicker.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

#if ANDROID
        if (Platform.CurrentActivity is not null)
            Platform.CurrentActivity.RequestedOrientation = ScreenOrientation.Portrait;
#endif
    }
}