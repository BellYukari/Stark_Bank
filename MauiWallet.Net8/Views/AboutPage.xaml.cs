namespace MauiWallet.Views;
public partial class AboutPage : PopupPage
{
    public AboutPage()
    {
        InitializeComponent();
        BindingContext = new AboutViewModel();
    }

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {
        await PopupNavigation.Instance.PopAsync();
    }
}
