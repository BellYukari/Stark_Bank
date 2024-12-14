using MauiWallet.Controls;

namespace MauiWallet.Views;
public partial class ExchangeRatePage : ContentPage
{
    private readonly ExchangeRateService _exchangeRateService;
    public ExchangeRatePage()
	{
		InitializeComponent();
        _exchangeRateService = new ExchangeRateService();
	}

    private async void LempirasToDollars_Clicked(object sender, EventArgs e)
    {
        var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync();

        if (exchangeRates != null)
        {
            Decimal lempiras = Decimal.Parse(txtLempiras.Text);
            Decimal totalDolares = 0;

            var hnlRate = exchangeRates.Data["HNL"];
            totalDolares = lempiras / hnlRate.Value;
            txtDolares.Text = "$ " + totalDolares.ToString("F4"); // Formatea a 4 decimales
            lblDolares.Text = "Total en Dolares";


        }
        else
        {
            txtDolares.Text = "Error";
            Console.WriteLine("error al recibir los tipos de cambio");
        }
    }

    private async void DollarsToLempiras_Clicked(object sender, EventArgs e)
    {
        var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync();

        if (exchangeRates != null)
        {
            Decimal dolares = Decimal.Parse(txtLempiras.Text);
            Decimal totalLempiras = 0;

            var hnlRate = exchangeRates.Data["HNL"];
            totalLempiras = dolares * hnlRate.Value;
            txtDolares.Text = "L " + totalLempiras.ToString("F4"); // Formatea a 4 decimales
            lblDolares.Text = "Total en Lempiras";


        }
        else
        {
            txtDolares.Text = "Error";
            Console.WriteLine("error al recibir los tipos de cambio");
        }
    }

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
    }

}