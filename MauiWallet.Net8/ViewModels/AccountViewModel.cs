
namespace MauiWallet.ViewModels;
public partial class AccountViewModel : ObservableObject
{

    [ObservableProperty]
    UserInfo _userInfo;

    public AccountViewModel()
    {
        DarkModeSwitchToggled = AppSettings.IsDarkMode;
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


    [RelayCommand]
    private async void UpdateAccount()
    {
        await PopupNavigation.Instance.PushAsync(new AccountUpdatePage());
    }

    [RelayCommand]
    private async void GotoPrivacy()
    {
        await Shell.Current.GoToAsync(nameof(PrivacyPolicyPage));
    }

    [RelayCommand]
  

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
        ThemeUtil.ApplyColorSet(AppSettings.SelectedPrimaryColorIndex);
    }
}
