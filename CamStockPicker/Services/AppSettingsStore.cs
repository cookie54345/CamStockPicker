namespace CamStockPicker.Services;

public enum FontSizeMode { S, M, L }
public enum ChangeDisplayMode { Dollars, Percent }

/// <summary>
/// Manages local storage and retrieval.
/// This includes dark mode settings, visual text sizes, and pricing metric.
/// </summary>

public class AppSettingsStore
{
    private const string DarkKey = "settings_dark";
    private const string FontKey = "settings_font";
    private const string ChangeKey = "settings_change_mode";

    public bool GetDarkMode() => Preferences.Get(DarkKey, false);
    public void SetDarkMode(bool value) => Preferences.Set(DarkKey, value);

    public FontSizeMode GetFontSizeMode()
        => Enum.TryParse(Preferences.Get(FontKey, FontSizeMode.M.ToString()), out FontSizeMode mode) ? mode : FontSizeMode.M;

    public void SetFontSizeMode(FontSizeMode mode) => Preferences.Set(FontKey, mode.ToString());

    public ChangeDisplayMode GetChangeDisplayMode()
        => Enum.TryParse(Preferences.Get(ChangeKey, ChangeDisplayMode.Dollars.ToString()), out ChangeDisplayMode mode) ? mode : ChangeDisplayMode.Dollars;

    public void SetChangeDisplayMode(ChangeDisplayMode mode) => Preferences.Set(ChangeKey, mode.ToString());
}