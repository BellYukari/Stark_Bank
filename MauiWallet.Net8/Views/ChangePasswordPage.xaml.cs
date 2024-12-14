namespace MauiWallet.Views;

public partial class ChangePasswordPage : ContentPage
{
    private ApiService _apiService;
    public ChangePasswordPage()
	{
		InitializeComponent();
        _apiService = new ApiService();

    }

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {

        string emailOTP = Preferences.Get("emailOTP", "");
        string codeOTP = Preferences.Get("codeOTP", "");

        var data = new
        {
            email = emailOTP,
            code = codeOTP,
            password = entryPassword.Text,
            password_confirmation = entryPasswordConfirm.Text
        };


        var result = await _apiService.PostSuccessAsync("/api/user/forget/reset/password", data);
        var status = result.IsSuccess ? "Exito" : "Error";

        if (result.IsSuccess)
        {

            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "LoginStyle1Page", ""));

        }
        else
        {
            await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
            //  await Navigation.PopAsync();
        }
    }

}