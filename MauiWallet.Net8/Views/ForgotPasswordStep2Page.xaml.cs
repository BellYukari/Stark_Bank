namespace MauiWallet.Views;

public partial class ForgotPasswordStep2Page : ContentPage
{
    private ApiService _apiService;
    private string email;
    private string C1, C2, C3, C4, C5, C6;
    public ForgotPasswordStep2Page(string correo)
	{
		InitializeComponent();
        _apiService = new ApiService();

        email = correo;
        entryCorreo.Text = correo;

    }

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
    }

    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {

        string codeOTP = OTPNumber1.Text + "" + OTPNumber2.Text + "" + OTPNumber3.Text + "" + OTPNumber4.Text + "" + OTPNumber5.Text + "" + OTPNumber6.Text;
        var data = new
        {
            email = entryCorreo.Text,
            code = codeOTP
        };

        Preferences.Set("emailOTP", entryCorreo.Text);
        Preferences.Set("codeOTP", codeOTP);

        var result = await _apiService.PostSuccessAsync("/api/user/forget/verify/otp", data);
        var status = result.IsSuccess ? "Exito" : "Error";

        //await DisplayAlert(result.IsSuccess ? "Success" : "Error", result.Message, "OK");

        if (result.IsSuccess)
        {
            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "ChangePasswordPage", entryCorreo.Text));
            //await Navigation.PushAsync(new SignUpStep3Page(entryCorreo.Text));

        }
        else
        {
            await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
        }
    }

    private async void ReenviarButton_Clicked(object sender, EventArgs e)
    {

        var data = new
        {
            email = entryCorreo.Text
        };

        var result = await _apiService.PostSuccessAsync("/api/user/forget/password", data);

        await DisplayAlert(result.IsSuccess ? "Exito" : "Error", result.Message, "OK");

    }

}