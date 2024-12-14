
using System.Text.Json;

namespace MauiWallet;

public partial class App : Application
{

    public static UserService UserService { get; private set; }
    public static string DeviceId { get; private set; }

    public App()
    {
        InitializeComponent();

        UserService = new UserService();

        CheckTokenValidity();

        #region Handlers

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.Layer.BorderColor = UIKit.UIColor.White.CGColor;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif __WINDOWS__
                handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
                handler.PlatformView.BorderThickness = new Windows.UI.Xaml.Thickness(0);
                handler.PlatformView.UnderlineThickness = new Windows.UI.Xaml.Thickness(0);
#endif
            }
        });

        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(BorderlessEditor), (handler, view) =>
        {
            if (view is BorderlessEditor)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif __IOS__ || __MACCATALYST__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.Layer.BorderColor = UIKit.UIColor.White.CGColor;
#elif __WINDOWS__
                handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;                
                handler.PlatformView.BorderThickness = new Thickness(0);
#endif
            }
        });

        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(BorderlessPicker), (handler, view) =>
        {
            if (view is BorderlessPicker)
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif __IOS__ || __MACCATALYST__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.Layer.BorderColor = UIKit.UIColor.White.CGColor;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif __WINDOWS__
                handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;                
                handler.PlatformView.BorderThickness = new Thickness(0);
#endif

            }
        });

        #endregion

    }

 

    protected override async void OnStart()
    {
        DeviceId = GetDeviceId();


        if (AppSettings.IsDarkMode)
            Application.Current.Resources.ApplyDarkTheme();
        else
            Application.Current.Resources.ApplyLightTheme();

        ThemeUtil.ApplyColorSet(AppSettings.SelectedPrimaryColorIndex);


    }

    private string GetDeviceId()
    {
        // Intenta obtener un ID guardado previamente
        var id = Preferences.Get("device_id", null);
        if (string.IsNullOrEmpty(id))
        {
            // Si no existe, genera uno nuevo
            id = Guid.NewGuid().ToString();
            Preferences.Set("device_id", id);
        }
        return id;
    }

    private void CheckTokenValidity()
    {
        var UserToken = Preferences.Get("UserToken", null);
        var TokenExpiration = Preferences.Get("TokenExpiration", DateTime.MinValue);
        var UserData = Preferences.Get("UserData", null);


        Console.WriteLine($"Response UserToken: {UserToken}");
        Console.WriteLine($"Response TokenExpiration: {TokenExpiration}");
        Console.WriteLine($"Response UserData: {UserData}");


        TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        DateTime utcNow = DateTime.UtcNow;
        DateTime tegucigalpaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, cstZone);


        if (tegucigalpaTime < TokenExpiration)
        {
            var userDataJson = Preferences.Get("UserData", string.Empty);
            if (!string.IsNullOrEmpty(userDataJson))
            {
                var userData = JsonSerializer.Deserialize<User>(userDataJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                UserService.CurrentUser = userData;

                if (AppSettings.IsFirstLaunching)
                {
                    AppSettings.IsFirstLaunching = false; 
                    MainPage = new NavigationPage(new DemoStartPage());
                }
                else
                    MainPage = new AppShell();

            }
        }
        else
        {
            Preferences.Remove("UserToken");
            Preferences.Remove("TokenExpiration");
            Preferences.Remove("UserData");
            Preferences.Remove("totBalance");
            Preferences.Remove("username");
            UserService.CurrentUser = null;

            if (AppSettings.IsFirstLaunching)
            {
                AppSettings.IsFirstLaunching = false;
                MainPage = new NavigationPage(new DemoStartPage());
            }
            else
                MainPage = new NavigationPage(new LoginPage());

        }
    }
}
