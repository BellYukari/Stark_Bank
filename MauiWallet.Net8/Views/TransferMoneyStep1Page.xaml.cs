using MauiWallet.Services;

namespace MauiWallet.Views;
public partial class TransferMoneyStep1Page : ContentPage
{
    private ApiService _apiService;
    public TransferMoneyStep1Page()
	{
		InitializeComponent();
        _apiService = new ApiService();

        string balanceString = Preferences.Get("totBalance", "0");
        decimal balance = decimal.Parse(balanceString, CultureInfo.InvariantCulture);
        entryBalance.Text = "Saldo disponible " + balance.ToString("F2")+ " HNL";

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

        var result = await _apiService.PostSuccessAsync("/api/user/send-money/exist", data);

        //await DisplayAlert(result.IsSuccess ? "Exito" : "Error", result.Message, "OK");

        var status = result.IsSuccess ? "Exito" : "Error";

        if (result.IsSuccess)
        {

            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "TransferMoneyStep2Page", entryCorreo.Text));
           //await Navigation.PushAsync(new SignUpStep2Page(entryCorreo.Text));

        }
        else
        {
            await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
          //  await Navigation.PopAsync();
        }
    }
}