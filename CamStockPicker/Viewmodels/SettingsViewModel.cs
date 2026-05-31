using System.Windows.Input;
using CamStockPicker.Services;

namespace CamStockPicker.ViewModels;

/// <summary>
/// Manages configuration settings views.
/// </summary>

public class SettingsViewModel : BaseViewModel
{
    private readonly AppSettingsStore _store;

    private bool isDarkMode;

    private FontSizeMode fontMode;
    private ChangeDisplayMode changeMode;

    // UI colours for buttons
    private static readonly Color SelectedBg = Color.FromArgb("#2F7CF6");
    private static readonly Color SelectedFg = Colors.White;
    private static readonly Color UnselectedBg = Colors.White;
    private static readonly Color UnselectedFg = Colors.Black;

    public bool IsDarkMode
    {
        get => isDarkMode;
        set
        {
            if (SetProperty(ref isDarkMode, value))
            {
                _store.SetDarkMode(value);
                ApplyTheme(value);
            }
        }
    }

    // Font segment colours
    public Color FontSmallBg => fontMode == FontSizeMode.S ? SelectedBg : UnselectedBg;
    public Color FontSmallFg => fontMode == FontSizeMode.S ? SelectedFg : UnselectedFg;

    public Color FontMediumBg => fontMode == FontSizeMode.M ? SelectedBg : UnselectedBg;
    public Color FontMediumFg => fontMode == FontSizeMode.M ? SelectedFg : UnselectedFg;

    public Color FontLargeBg => fontMode == FontSizeMode.L ? SelectedBg : UnselectedBg;
    public Color FontLargeFg => fontMode == FontSizeMode.L ? SelectedFg : UnselectedFg;

    // Change mode segment colors
    public Color DollarsBg => changeMode == ChangeDisplayMode.Dollars ? SelectedBg : UnselectedBg;
    public Color DollarsFg => changeMode == ChangeDisplayMode.Dollars ? SelectedFg : UnselectedFg;

    public Color PercentBg => changeMode == ChangeDisplayMode.Percent ? SelectedBg : UnselectedBg;
    public Color PercentFg => changeMode == ChangeDisplayMode.Percent ? SelectedFg : UnselectedFg;

    public ICommand SetFontSmallCommand { get; }
    public ICommand SetFontMediumCommand { get; }
    public ICommand SetFontLargeCommand { get; }

    public ICommand SetChangeDollarsCommand { get; }
    public ICommand SetChangePercentCommand { get; }

    public SettingsViewModel(AppSettingsStore store)
    {
        Title = "Settings";
        _store = store;

        isDarkMode = _store.GetDarkMode();
        fontMode = _store.GetFontSizeMode();
        changeMode = _store.GetChangeDisplayMode();

        ApplyTheme(isDarkMode);
        ApplyFont(fontMode);

        SetFontSmallCommand = new Command(() => SetFont(FontSizeMode.S));
        SetFontMediumCommand = new Command(() => SetFont(FontSizeMode.M));
        SetFontLargeCommand = new Command(() => SetFont(FontSizeMode.L));

        SetChangeDollarsCommand = new Command(() => SetChangeMode(ChangeDisplayMode.Dollars));
        SetChangePercentCommand = new Command(() => SetChangeMode(ChangeDisplayMode.Percent));

        RaiseSegmentColors();
    }

    private void SetFont(FontSizeMode mode)
    {
        fontMode = mode;
        _store.SetFontSizeMode(mode);

        ApplyFont(mode);

        RaiseSegmentColors();
    }

    private void SetChangeMode(ChangeDisplayMode mode)
    {
        changeMode = mode;
        _store.SetChangeDisplayMode(mode);
        RaiseSegmentColors();
    }

    private void RaiseSegmentColors()
    {
        OnPropertyChanged(nameof(FontSmallBg));
        OnPropertyChanged(nameof(FontSmallFg));
        OnPropertyChanged(nameof(FontMediumBg));
        OnPropertyChanged(nameof(FontMediumFg));
        OnPropertyChanged(nameof(FontLargeBg));
        OnPropertyChanged(nameof(FontLargeFg));

        OnPropertyChanged(nameof(DollarsBg));
        OnPropertyChanged(nameof(DollarsFg));
        OnPropertyChanged(nameof(PercentBg));
        OnPropertyChanged(nameof(PercentFg));
    }

    private static void ApplyTheme(bool dark)
    {
        if (Application.Current is null) return;
        Application.Current.UserAppTheme = dark ? AppTheme.Dark : AppTheme.Light;
    }

    private static void ApplyFont(FontSizeMode mode)
    {
        if (Application.Current?.Resources is null) return;

        // Font size mode
        var baseSize = mode switch
        {
            FontSizeMode.S => 12d,
            FontSizeMode.M => 16d,
            FontSizeMode.L => 28d,
            _ => 16d
        };

        Application.Current.Resources["AppFontSize"] = baseSize;
        Application.Current.Resources["AppFontSizeTitle"] = baseSize + 14; // 26 / 30 / 42
        Application.Current.Resources["AppFontSizeBig"] = baseSize + 30;   // 42 / 46 / 58
    }
}