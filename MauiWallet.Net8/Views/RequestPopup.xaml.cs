namespace MauiWallet.Views;
public partial class RequestPopup : PopupPage
{
    private string screen;
    private string email;

    public RequestPopup(string title, string descri, string screens, string correo)
	{
		InitializeComponent();
        entrytitle.Text = title;
        entrydescri.Text = descri;
        screen = screens;
        email = correo;

        entryRecibo.IsVisible = false;

        if (screen == "HomePage")
        {
            entryRecibo.IsVisible = false;
        }
        else
        {
            entryRecibo.IsVisible = false;
        }

    }

    private async void AceptarButton_Clicked(object sender, EventArgs e)
    {
        if (screen == "AccountPage")
        {            
            await Navigation.PushAsync(new AccountPage());
        }
        if (screen == "SignUpStep2Page")
        {
            await Navigation.PushAsync(new SignUpStep2Page(email));
        }
        if (screen == "SignUpStep3Page")
        {
            await Navigation.PushAsync(new SignUpStep3Page(email));
        }

        if (screen == "TransferMoneyStep2Page")
        {
            await Application.Current.MainPage.Navigation.PushAsync(new TransferMoneyStep2Page(email));
            //await Navigation.PushAsync(new TransferMoneyStep2Page(email));
        }

        if (screen == "ForgotPasswordStep2Page")
        {
            await Navigation.PushAsync(new ForgotPasswordStep2Page(email));
        }

        if (screen == "LoginStyle1Page")
        {
            await Navigation.PushAsync(new LoginPage());
        }

        if (screen == "ChangePasswordPage")
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }

        if (screen == "BillPaymentPage")
        {
            await Navigation.PushAsync(new BillPaymentPage());
        }

        if (screen == "MobileTopupPage")
        {
            await Navigation.PushAsync(new MobileTopupPage());
        }

        if (screen == "HomePage")
        {
            Application.Current.MainPage = new AppShell();
        }


        await PopupNavigation.Instance.PopAsync();
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        await PopupNavigation.Instance.PopAsync();
    }

    private async void ViewReceiptButton_Clicked(object sender, EventArgs e)
    {
       // await Navigation.PushAsync(new TransferReceiptPage());
    }
}