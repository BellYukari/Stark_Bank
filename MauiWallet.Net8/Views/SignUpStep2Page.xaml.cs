namespace MauiWallet.Views;

public partial class SignUpStep2Page : ContentPage
{
    private ApiService _apiService;
    private string email;
    private string C1, C2, C3, C4, C5, C6;
    public SignUpStep2Page(string correo)
	{
		InitializeComponent();
        _apiService = new ApiService();

        email = correo;
        entryCorreo.Text = correo;

    }

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {
        var data = new
        {
            email = entryCorreo.Text,
            code = OTPNumber1.Text + "" + OTPNumber2.Text + "" + OTPNumber3.Text + "" + OTPNumber4.Text + "" + OTPNumber5.Text + "" + OTPNumber6.Text
        };

        var result = await _apiService.PostSuccessAsync("/api/user/register/verify/otp", data);
        var status = result.IsSuccess ? "Exito" : "Error";


        if (result.IsSuccess)
        {
            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "SignUpStep3Page", entryCorreo.Text));

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
            email = entryCorreo.Text,
            agree = 1
        };

        var result = await _apiService.PostSuccessAsync("/api/user/register/send/otp", data);

        await DisplayAlert(result.IsSuccess ? "Exito" : "Error", result.Message, "OK");
    }


}