using CommunityToolkit.Maui.Core;

namespace MauiWallet.Settings;
public static class ThemeUtil
{
    public static void ApplyDarkTheme(this ResourceDictionary resources)
    {
        if (resources != null)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                var lightTheme = mergedDictionaries.OfType<LightTheme>().FirstOrDefault();
                if (lightTheme != null)
                {
                    mergedDictionaries.Remove(lightTheme);
                }
                mergedDictionaries.Add(new DarkTheme());
            }
        }
    }

    public static void ApplyLightTheme(this ResourceDictionary resources)
    {
        if (resources != null)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                var darkTheme = mergedDictionaries.OfType<DarkTheme>().FirstOrDefault();
                if (darkTheme != null)
                {
                    mergedDictionaries.Remove(darkTheme);
                }
                mergedDictionaries.Add(new LightTheme());
            }
        }
    }

    public static void ApplyColorSet(int index)
    {
        switch (index)
        {
            case 0:
                ApplyColorSet1();
                break;
            case 1:
                ApplyColorSet2();
                break;
            case 2:
                ApplyColorSet3();
                break;
            case 3:
                ApplyColorSet4();
                break;
            case 4:
                ApplyColorSet5();
                break;
        }
    }

    public static void ApplyColorSet1()
    {
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption1", out var primaryColorOption1);
        Application.Current.Resources["LinearGradientStartColor"] = Color.FromArgb("#5581F1");
        Application.Current.Resources["LinearGradientEndColor"] = Color.FromArgb("#1153FC");
        Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption1;
        Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#1a5ac6");
    }

    public static void ApplyColorSet2()
    {
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption2", out var primaryColorOption2);
        Application.Current.Resources["LinearGradientStartColor"] = Color.FromArgb("#C165DD");
        Application.Current.Resources["LinearGradientEndColor"] = Color.FromArgb("#5C27FE");
        Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption2;
        Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#401BB1");
    }

    public static void ApplyColorSet3()
    {
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption3", out var primaryColorOption3);
        Application.Current.Resources["LinearGradientStartColor"] = Color.FromArgb("#2AFEB7");
        Application.Current.Resources["LinearGradientEndColor"] = Color.FromArgb("#08C792");
        Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption3;
        Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#056c56");
    }

    public static void ApplyColorSet4()
    {
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption4", out var primaryColorOption4);
        Application.Current.Resources["LinearGradientStartColor"] = Color.FromArgb("#FDC830");
        Application.Current.Resources["LinearGradientEndColor"] = Color.FromArgb("#F37335");
        Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption4;
        Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#996C1E");
    }

    public static void ApplyColorSet5()
    {
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption5", out var primaryColorOption5);
        Application.Current.Resources["LinearGradientStartColor"] = Color.FromArgb("#F5515F");
        Application.Current.Resources["LinearGradientEndColor"] = Color.FromArgb("#A1051D");
        Application.Current.Resources["PrimaryColor"] = (Color)primaryColorOption5;
        Application.Current.Resources["PrimaryDarkColor"] = Color.FromArgb("#A43106");
    }
}
