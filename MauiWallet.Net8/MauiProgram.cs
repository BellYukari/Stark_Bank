#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

#if ANDROID
using AndroidX.Core.View;
using AndroidX.AppCompat.App;
using Android.Views;
using Android.OS;
using AView = Android.Views.View;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#endif

using Microsoft.Maui.LifecycleEvents;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using RGPopup.Maui.Extensions;
using SkiaSharp.Views.Maui.Controls.Hosting;


namespace MauiWallet
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .UseSkiaSharp(true)
#if !WINDOWS
            .UseMauiMaps()
#endif
            .UseBarcodeReader()
            .RegisterServices()
            .UseMauiRGPopup(config =>
            {
                config.BackPressHandler = null;
                config.FixKeyboardOverlap = true;
            })
            .RegisterViews()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Poppins-Regular.otf", "RegularFont");
                fonts.AddFont("Poppins-Medium.otf", "MediumFont");
                fonts.AddFont("Poppins-SemiBold.otf", "SemiBoldFont");
                fonts.AddFont("Poppins-Bold.otf", "BoldFont");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font-Awesome-Solid.otf", "FontAwesomeSolid");

                fonts.AddFont("fa-solid-900.ttf", "FaPro");
                fonts.AddFont("fa-brands-400.ttf", "FaBrands");
                fonts.AddFont("fa-regular-400.ttf", "FaRegular");
                fonts.AddFont("line-awesome.ttf", "LineAwesome");
                fonts.AddFont("material-icons-outlined-regular.otf", "MaterialDesign");
                fonts.AddFont("ionicons.ttf", "IonIcons");
                fonts.AddFont("icon.ttf", "MauiKitIcons");
            })
            .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler(typeof(CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));
                    handlers.AddHandler(typeof(CameraView), typeof(CameraViewHandler));
                    handlers.AddHandler(typeof(BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
                })
            .ConfigureLifecycleEvents(events =>
             {
            #if ANDROID
                 events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

                 static void MakeStatusBarTranslucent(Android.App.Activity activity)
                 {
                     //activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);
                     activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
                     //activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                     activity.Window.SetNavigationBarColor(Android.Graphics.Color.Black);
                 }
            #endif
             });

            builder.Services.AddLocalization();

#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnWindowCreated(window =>
                    {
                        IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                        AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);

                        window.ExtendsContentIntoTitleBar = false; 
                        winuiAppWindow.SetPresenter(AppWindowPresenterKind.Default);

                        var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(win32WindowsId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);

                        int width = displayArea.WorkArea.Width * 2 / 3;
                        int height = displayArea.WorkArea.Height - 10;

                        winuiAppWindow.MoveAndResize(new RectInt32(15, 10, width, height));
                    });
                });
            });
#endif


            return builder.Build();
        }

        private static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IBiometricAuthenticationService, BiometricAuthenticationServicenk>();
            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<LoginPage>();
            mauiAppBuilder.Services.AddTransient<AppShell>();

            return mauiAppBuilder;
        }

      
    }
}
