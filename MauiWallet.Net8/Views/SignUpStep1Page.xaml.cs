using MauiWallet.Services;
using Microsoft.Maui.ApplicationModel.Communication;

namespace MauiWallet.Views;
public partial class SignUpStep1Page : ContentPage
{
    private ApiService _apiService;
    public SignUpStep1Page()
	{
		InitializeComponent();
        _apiService = new ApiService();
    } 

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
    }

    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {

        var data = new
        {
            email = entryCorreo.Text,
            agree = 1
        };

        var result = await _apiService.PostSuccessAsync("/api/user/register/send/otp", data);

        //await DisplayAlert(result.IsSuccess ? "Exito" : "Error", result.Message, "OK");

        var status = result.IsSuccess ? "Exito" : "Error";
         
        if (result.IsSuccess)
        {

            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "SignUpStep2Page",entryCorreo.Text));
           //await Navigation.PushAsync(new SignUpStep2Page(entryCorreo.Text));

        }
        else
        {
            await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
          //  await Navigation.PopAsync();
        }
    }
}