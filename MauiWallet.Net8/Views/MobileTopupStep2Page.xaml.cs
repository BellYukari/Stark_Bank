using Microsoft.Maui.Controls;
using MauiWallet.Services;

namespace MauiWallet.Views;

public partial class MobileTopupStep2Page : ContentPage
{
    private ApiService _apiService;
    public string _codigo;
    public string selectedMonto;
    public MobileTopupStep2Page(string codigo, string nombre)
    {
        InitializeComponent();
        //BindingContext = new TransferMoneyViewModel();
        _apiService = new ApiService();

        _codigo = codigo;

        string balanceString = Preferences.Get("totBalance", "0");
        decimal balance = decimal.Parse(balanceString, CultureInfo.InvariantCulture);
        entryBalance.Text = "L. " + balance.ToString("F2");

        entryTitulo.Text = "Recarga " + nombre;
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedIndex != -1)
        {
            selectedMonto = picker.Items[picker.SelectedIndex];
        }
    }

    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {

        var data = new
        {
            topup_type = _codigo,
            mobile_code = "504",
            mobile_number = entryBill_number.Text,
            amount = selectedMonto
        };

        var result = await _apiService.PostSuccessAsync("/api/user/mobile-topup/confirmed", data);
        var status = result.IsSuccess ? "Exito" : "Error";

        if (result.IsSuccess)
        {
            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "MobileTopupPage", ""));
        }
        else
        {
            await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
        }
    }



}