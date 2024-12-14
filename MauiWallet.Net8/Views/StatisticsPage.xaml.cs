using Microsoft.Maui.Controls;
using MauiWallet.Services;
using System;
using System.Text.Json;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Threading.Tasks;
using MauiWallet.Models;

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Maui.Controls.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;

namespace MauiWallet.Views;

public partial class StatisticsPage : ContentPage
{
    private AuthService _authService;
    private readonly HttpClient _httpClient;
    private ApiService _apiService;
    public SolidColorPaint _legendTextPaint;
    private decimal _totalIngresos;
    private decimal _totalGastos;

    private string[] days;

    private string[] weeks;

    private string[] months;

    private string[] section;

    private ObservableCollection<ISeries> _weekChart;

    public ObservableCollection<ISeries> ChartData { get; private set; }

    public ObservableCollection<TransactionData> DataSource { get; private set; }



    private int _selectedIndex;
    public int SelectedIndex
    {
        get
        {
            return _selectedIndex;
        }
        set
        {
            _selectedIndex = value;
         
        }
    }


    public StatisticsPage()
	{
		InitializeComponent();
        _authService = new AuthService();
        _httpClient = new HttpClient();
        _apiService = new ApiService();

        ChartData = new ObservableCollection<ISeries>();
        DataSource = new ObservableCollection<TransactionData>()
        {
            new TransactionData() { Duration = "Semana" },
            new TransactionData() { Duration = "Mes" },
            new TransactionData() { Duration = "Año" },
        };

        //GetTransactionsIngresosAsync(0);
         //GetTransactionsIngresosAsync(1);
    }


    private async void Ingresos_Clicked(object sender, EventArgs e)
    {
        TransactionsCollectionView.ItemsSource = null;
        await   GetTransactionsIngresosAsync(1);
    }

    private async void Gastos_Clicked(object sender, EventArgs e)
    {
        TransactionsCollectionView.ItemsSource = null;
        await GetTransactionsIngresosAsync(0);
    }

    private async void OnTransactionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedTransaction = e.CurrentSelection.FirstOrDefault() as TransactionItem;
            if (selectedTransaction != null)
            {
                string trxValue = selectedTransaction.trx;
                string amountValue = selectedTransaction.request_amount;
                string date_timeValue = FormatDate(selectedTransaction.date_time);
                Console.WriteLine($"Transaction selected: {trxValue}");
                await Navigation.PushAsync(new ReceiptPage(trxValue, amountValue, date_timeValue));

            }
        }

    ((CollectionView)sender).SelectedItem = null;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetTransactionsIngresosAsync(1);
    }

    public async Task GetTransactionsIngresosAsync(int idT)
    {
        try
        {
            var UserToken = Preferences.Get("UserToken", null);
            string url = ApiService.apiURL + "/api/user/transactions?lang=es";

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserToken);

            var response = await _httpClient.GetStringAsync(url);
            Console.WriteLine($"apiResponseLista: {response}");

            var apiResponse = JsonSerializer.Deserialize<ApiResponseTransactions>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Procesar las transacciones obtenidas
            var transactions = new List<TransactionItem>();

            if (idT == 1)
            {

                if (apiResponse?.Data?.Transactions?.send_money != null)
                {
                    transactions.AddRange(apiResponse.Data.Transactions.send_money
                        .Where(t => t.type == "RECEIVED")
                        .Select(t => new TransactionItem
                        {
                            ImageIcon = MauiKitIcons.Cash,
                            IconColor = "#06846A",
                            trx = "Recibio: " + t.trx,
                            notrx = t.trx,
                            type = t.type,
                            request_amount = t.recipient_received,
                            date_time = t.date_time,
                            FormattedDateTime = FormatDate(t.date_time),
                            IsCredited = true
                        }));
                }


                if (apiResponse?.Data?.Transactions?.add_money != null)
                {
                    transactions.AddRange(apiResponse.Data.Transactions.add_money.Select(t => new TransactionItem
                    {
                        ImageIcon = MauiKitIcons.Cash,
                        IconColor = "#06846A",
                        trx = "Recargo: " + t.trx,
                        request_amount = t.request_amount,
                        date_time = t.date_time,
                        FormattedDateTime = FormatDate(t.date_time),
                        IsCredited = true
                    }));
                }

                if (apiResponse?.Data?.Transactions?.virtual_card != null)
                {
                    transactions.AddRange(apiResponse.Data.Transactions.virtual_card.Select(t => new TransactionItem
                    {
                        ImageIcon = MauiKitIcons.Cash,
                        IconColor = "#06846A",
                        trx = "TV: " + t.trx,
                        request_amount = t.request_amount,
                        date_time = t.date_time,
                        FormattedDateTime = FormatDate(t.date_time),
                        IsCredited = true
                    }));
                }

                if (apiResponse?.Data?.Transactions?.add_sub_balance != null)
                {
                    transactions.AddRange(apiResponse.Data.Transactions.add_sub_balance.Select(t => new TransactionItem
                    {
                        ImageIcon = MauiKitIcons.Cash,
                        IconColor = "#06846A",
                        trx = "Admin: " + t.trx,
                        request_amount = t.request_amount,
                        date_time = t.date_time,
                        FormattedDateTime = FormatDate(t.date_time),
                        IsCredited = true
                    }));
                }

                

            }
            else
            {
                if (apiResponse?.Data?.Transactions?.send_money != null)
                {
                    transactions.AddRange(apiResponse.Data.Transactions.send_money
                        .Where(t => t.type == "SEND") 
                        .Select(t => new TransactionItem
                        {
                            ImageIcon = MauiKitIcons.Cash,
                            IconColor = "#d54381",
                            trx = "Envio: " + t.trx,
                            notrx = t.trx,
                            type = t.type,
                            request_amount = t.recipient_received,
                            date_time = t.date_time,
                            FormattedDateTime = FormatDate(t.date_time),
                            IsCredited = false
                        }));
                }


                if (apiResponse?.Data?.Transactions?.bill_pay != null)
                {
                    transactions.AddRange(apiResponse.Data.Transactions.bill_pay.Select(t => new TransactionItem
                    {
                        ImageIcon = MauiKitIcons.Cash,
                        IconColor = "#d54381",
                        trx = t.bill_type,
                        request_amount = t.request_amount,
                        date_time = t.date_time,
                        FormattedDateTime = t.trx + " - " + FormatDate(t.date_time),
                        IsCredited = false
                    }));
                }

                // _totalGastos = transactions.Sum(t => Convert.ToDecimal(t.request_amount));

            }

            //string totaltIngresosString = _totalIngresos.ToString("C", new CultureInfo("es-ES"));
            //string totalGastosString = _totalGastos.ToString("C", new CultureInfo("es-ES"));

            /*_totalIngresos = transactions.Sum(t => Convert.ToDecimal(t.request_amount));
            decimal balance = _totalIngresos;

            entryIngresos.Text = "L. " + balance.ToString("F2");
            entryGastos.Text = "1200";*/

            // Ordenar todas las transacciones por fecha en orden descendente
            var orderedTransactions = transactions.OrderByDescending(t => DateTime.Parse(t.date_time)).ToList();

            // Asignar la lista ordenada al CollectionView
            TransactionsCollectionView.ItemsSource = orderedTransactions;


            /*decimal totalAmount = transactions
            .Where(t => decimal.TryParse(t.request_amount, out _))
            .Sum(t => Convert.ToDecimal(t.request_amount));
            Console.WriteLine($"Gran Total: {totalAmount}");

            // Convertir el gran total a una cadena formateada
            string totalAmountString = totalAmount.ToString("C", new CultureInfo("es-ES"));*/

            // Mostrar el gran total en la interfaz de usuario (ejemplo con Label)
            //entryIngresos.Text = $"L. {totalAmountString}";

            entryIngresos.Text = "L. 5,500";
            entryGastos.Text = "L. 1,200";




        }
        catch (Exception ex)
        {
            Console.WriteLine($"ErrorExiste: {ex.Message}");
        }
    }

    public ISeries[] LineSeries { get; set; } =
    {
        new LineSeries<int>
        {
            Values = new int[] { 4, 4, 7, 2, 8 },
            Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(90)),
            //Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 4 },
            LineSmoothness = 1,
        },
        new LineSeries<int>
        {
            Values = new int[] { 7, 5, 3, 2, 6 },
            Fill = new SolidColorPaint(SKColors.Red.WithAlpha(90)), 
            //Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 4 },
            LineSmoothness = 1,
        }
    };

    public string FormatDate(string dateTime)
    {
        if (DateTime.TryParse(dateTime, out DateTime parsedDate))
        {
            return parsedDate.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("es-ES"));
        }
        return dateTime; // Devuelve la cadena original si no se puede parsear
    }


    public class TransactionItem
    {
        public string ImageIcon { get; set; }
        public string IconColor { get; set; }
        public string trx { get; set; }
        public string notrx { get; set; }
        public string type { get; set; }
        public string request_amount { get; set; }
        public string date_time { get; set; }
        public string FormattedDateTime { get; set; }
        public bool IsCredited { get; set; }
    }

    public class ApiResponseTransactions
    {
        public Message Message { get; set; }
        public Data Data { get; set; }
    }

    public class Message
    {
        public List<string> Success { get; set; }
    }

    public class Data
    {
        public Dictionary<string, string> TransactionTypes { get; set; }
        public Transactions Transactions { get; set; }
    }

    public class Transactions
    {
        public List<Transaction> bill_pay { get; set; }
        public List<Transaction> mobile_top_up { get; set; }
        public List<Transaction> add_money { get; set; }
        public List<Transaction> money_out { get; set; }
        public List<Transaction> send_money { get; set; }
        public List<Transaction> money_in { get; set; }
        public List<Transaction> agent_money_out { get; set; }
        public List<Transaction> request_money { get; set; }
        public List<Transaction> virtual_card { get; set; }
        public List<Transaction> pay_link { get; set; }
        public List<Transaction> pay_user_pay_link { get; set; }
        public List<Transaction> Remittance { get; set; }
        public List<Transaction> merchant_payment { get; set; }
        public List<Transaction> make_payment { get; set; }
        public List<Transaction> gift_cards { get; set; }
        public List<Transaction> add_sub_balance { get; set; }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public string trx { get; set; }
        public string gateway_name { get; set; }
        public string type { get; set; }
        public string transaction_type { get; set; }
        public string request_amount { get; set; }
        public string recipient_received { get; set; }
        public string payable { get; set; }
        public string bill_type { get; set; }
        public string bill_month { get; set; }
        public string bill_number { get; set; }
        public string exchange_rate { get; set; }
        public string total_charge { get; set; }
        public string current_balance { get; set; }
        public bool Confirm { get; set; }
        public List<object> dynamic_inputs { get; set; }
        public bool confirm_url { get; set; }
        public string Status { get; set; }
        public string date_time { get; set; }
        public StatusInfo status_info { get; set; }
        public string rejection_reason { get; set; }
    }

    public class StatusInfo
    {
        public int Success { get; set; }
        public int Pending { get; set; }
        public int Rejected { get; set; }
    }


}