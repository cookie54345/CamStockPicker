using CamStockPicker.ViewModels;

namespace CamStockPicker.Views;

public partial class WatchlistPage : ContentPage
{
    public WatchlistPage(WatchlistViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is WatchlistViewModel vm)
            vm.RefreshCommand.Execute(null);
    }
}