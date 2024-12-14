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

namespace MauiWallet.Views;

public partial class BillPaymentPage : ContentPage
{
    private AuthService _authService;
    private readonly HttpClient _httpClient;
    private ApiService _apiService;

    public BillPaymentPage()
	{
		InitializeComponent();
        _authService = new AuthService();
        _httpClient = new HttpClient();
        _apiService = new ApiService();
        GetTransactionsAsync();

    }

    private async void PayNow_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BillPaymentStep2Page("7", "ENEE", "250"));
    }

    private async void ENEE_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BillPaymentStep2Page("7", "ENEE", ""));
    }

    private async void AguasSPS_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BillPaymentStep2Page("6", "Aguas SPS",""));
    }

    private async void Internet_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BillPaymentStep2Page("5", "Internet",""));
    }

    private async void Telefono_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BillPaymentStep2Page("4", "Teléfono",""));
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await GetTransactionsAsync();
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

    public async Task GetTransactionsAsync()
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


            // Ordenar todas las transacciones por fecha en orden descendente
            var orderedTransactions = transactions.OrderByDescending(t => DateTime.Parse(t.date_time)).ToList();

            // Asignar la lista ordenada al CollectionView
            TransactionsCollectionView.ItemsSource = orderedTransactions;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"ErrorExiste: {ex.Message}");
        }
    }

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
        public string transaction_type { get; set; }
        public string request_amount { get; set; }
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