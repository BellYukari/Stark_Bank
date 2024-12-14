namespace MauiWallet;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(DemoStartPage), typeof(DemoStartPage));
        Routing.RegisterRoute(nameof(DemoWalkthroughPage), typeof(DemoWalkthroughPage));
        Routing.RegisterRoute(nameof(MobileTopupPage), typeof(MobileTopupPage));
        Routing.RegisterRoute(nameof(TransferMoneyStep1Page), typeof(TransferMoneyStep1Page));
        Routing.RegisterRoute(nameof(PaymentConfirmPage), typeof(PaymentConfirmPage));
        Routing.RegisterRoute(nameof(ScanQrPayPage), typeof(ScanQrPayPage));
        Routing.RegisterRoute(nameof(NotificationsPage), typeof(NotificationsPage));
        Routing.RegisterRoute(nameof(PrivacyPolicyPage), typeof(PrivacyPolicyPage));
        Routing.RegisterRoute(nameof(ReceiveMoneyPage), typeof(ReceiveMoneyPage));
        Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
    }

    private async void OnSetting_Tapped(object sender, TappedEventArgs e)
    {
        var popup = new ThemeSettingsPopupPage();
        await PopupNavigation.Instance.PushAsync(popup);
    }

    private async void OnNotification_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new NotificationsPage());
    }
}
