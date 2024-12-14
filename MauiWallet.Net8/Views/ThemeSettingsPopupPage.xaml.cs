namespace MauiWallet.Views;
public partial class ThemeSettingsPopupPage : PopupPage
{
    public ThemeSettingsPopupPage()
    {
        InitializeComponent();
        BindingContext = new ThemeSettingsPopupViewModel();

        CreatePrimaryColorCollection();
    }

    void CreatePrimaryColorCollection()
    {
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption1", out var primaryColorOption1);
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption2", out var primaryColorOption2);
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption3", out var primaryColorOption3);
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption4", out var primaryColorOption4);
        Application.Current.Resources.TryGetValue("ThemePrimaryColorOption5", out var primaryColorOption5);

        var colorItems = new List<PrimaryColorItem>
        {
            new PrimaryColorItem()
            {
                Index = 0,
                Color = (Color)primaryColorOption1
            },
            new PrimaryColorItem()
            {
                Index = 1,
                Color = (Color)primaryColorOption2
            },
            new PrimaryColorItem()
            {
                Index = 2,
                Color = (Color)primaryColorOption3
            },
            new PrimaryColorItem()
            {
                Index = 3,
                Color = (Color)primaryColorOption4
            },
            new PrimaryColorItem()
            {
                Index = 4,
                Color = (Color)primaryColorOption5
            }
        };

        var viewCollection = new ObservableCollection<PrimaryColorItem>();

        foreach (var colorItem in colorItems)
        {
            viewCollection.Add(colorItem);
        }

        colorPaletteCollectionView.ItemsSource = viewCollection;
    }

    async void OnCloseSetting_Tapped(object sender, TappedEventArgs e)
    {
        await PopupNavigation.Instance.PopAsync();
    }
}
