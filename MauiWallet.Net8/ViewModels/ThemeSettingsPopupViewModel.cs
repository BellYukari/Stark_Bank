namespace MauiWallet.ViewModels;
public partial class ThemeSettingsPopupViewModel : ObservableObject
{
    public ThemeSettingsPopupViewModel()
    {

    }

    private PrimaryColorItem selectedPrimaryColorItem;
    public PrimaryColorItem SelectedPrimaryColorItem
    {
        get => selectedPrimaryColorItem;
        set
        {
            SetProperty(ref selectedPrimaryColorItem, value);
            AppSettings.SelectedPrimaryColorItem = value;
            ThemeUtil.ApplyColorSet(value.Index);
        }
    }

    private int selectedPrimaryColor;
    public int SelectedPrimaryColor
    {
        get => selectedPrimaryColor;
        set
        {
            SetProperty(ref selectedPrimaryColor, value);
            AppSettings.SelectedPrimaryColorIndex = value;
        }
    }

    private bool darkModeSwitchToggled;
    public bool DarkModeSwitchToggled
    {
        get => darkModeSwitchToggled;
        set
        {
            SetProperty(ref darkModeSwitchToggled, value);
            SetTheme(value);
        }
    }

    public void SetTheme(bool isDarkMode)
    {
        if (isDarkMode)
        {
            Application.Current.Resources.ApplyDarkTheme();
            AppSettings.IsDarkMode = true;
        }
        else
        {
            Application.Current.Resources.ApplyLightTheme();
            AppSettings.IsDarkMode = false;
        }
    }
}
