namespace MauiWallet.Views;
public partial class ForgotPasswordPage : ContentPage
{
    private ApiService _apiService;
    public ForgotPasswordPage()
	{
		InitializeComponent();
        _apiService = new ApiService();
    }

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {

        var data = new
        {
            email = entryCorreo.Text
        };

        var result = await _apiService.PostSuccessAsync("/api/user/forget/password", data);

        //await DisplayAlert(result.IsSuccess ? "Exito" : "Error", result.Message, "OK");

        var status = result.IsSuccess ? "Exito" : "Error";

        if (result.IsSuccess)
        {

            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "ForgotPasswordStep2Page", entryCorreo.Text));

        }
        else
        {
            await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
            //  await Navigation.PopAsync();
        }
    }

}