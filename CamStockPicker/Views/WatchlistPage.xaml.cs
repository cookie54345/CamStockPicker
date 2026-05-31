using CamStockPicker.ViewModels;

namespace CamStockPicker.Views;

public partial class WatchlistPage : ContentPage
{
    public WatchlistPage(WatchlistViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}