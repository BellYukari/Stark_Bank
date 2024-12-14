using Microsoft.Maui.Controls;
using MauiWallet.Services;

namespace MauiWallet.Views;

public partial class BillPaymentStep2Page : ContentPage
{
    private ApiService _apiService;
    public string _codigo;
    public string xvalor;
    public string selectedMonth;
    public BillPaymentStep2Page(string codigo, string nombre, string valor)
    {
        InitializeComponent();
        //BindingContext = new TransferMoneyViewModel();
        _apiService = new ApiService();

        _codigo = codigo;
        xvalor = valor;

        if (xvalor != "")
        {
            entryMonto.Text = xvalor;
        }

        string balanceString = Preferences.Get("totBalance", "0");
        decimal balance = decimal.Parse(balanceString, CultureInfo.InvariantCulture);
        entryBalance.Text = "L. " + balance.ToString("F2");

        entryTitulo.Text = "Pago de " + nombre;
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedIndex != -1)
        {
            selectedMonth = picker.Items[picker.SelectedIndex];
        }
    }

    private async void ContinueButton_Clicked(object sender, EventArgs e)
    {

        var data = new
        {
            bill_type = _codigo,
            bill_number = entryBill_number.Text,
            amount = entryMonto.Text,
            bill_month = selectedMonth,
            biller_item_type = "MANUAL",
        };

        var result = await _apiService.PostSuccessAsync("/api/user/bill-pay/confirmed", data);
        var status = result.IsSuccess ? "Exito" : "Error";

        if (result.IsSuccess)
        {
            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "BillPaymentPage", ""));
        }
        else
        {
            await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
        }
    }



}