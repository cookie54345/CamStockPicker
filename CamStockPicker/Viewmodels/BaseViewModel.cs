using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CamStockPicker.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    bool isBusy;
    string title = string.Empty;

    public bool IsBusy
    {
        get => isBusy;
        set => SetProperty(ref isBusy, value);
    }

    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;
        backingStore = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}