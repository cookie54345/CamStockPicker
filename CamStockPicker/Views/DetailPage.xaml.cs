using CamStockPicker.ViewModels;
#if ANDROID
using Android.Content.PM;
using Microsoft.Maui.ApplicationModel;
#endif

namespace CamStockPicker.Views;

public partial class DetailPage : ContentPage
{
    public DetailPage(DetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

#if ANDROID
        if (Platform.CurrentActivity is not null)
            Platform.CurrentActivity.RequestedOrientation = ScreenOrientation.Landscape;
#endif
    }
}