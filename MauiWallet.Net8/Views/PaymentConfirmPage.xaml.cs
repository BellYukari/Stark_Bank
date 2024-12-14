using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiWallet.Views;

public partial class PaymentConfirmPage : ContentPage
{
    private ApiResponseUserExist user;
    private ApiService _apiService;
    private string _email;
    private string _monto;
    public PaymentConfirmPage(string email, string montoEnvio, string nota)
	{
		InitializeComponent();
        _apiService = new ApiService();

        _email = email;
        _monto = montoEnvio;
        entryMonto1.Text = "L. "+montoEnvio;
        entryMonto.Text = "L. "+montoEnvio;
        entryNota.Text = nota;

        TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        DateTime utcNow = DateTime.UtcNow;
        DateTime tegucigalpaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, cstZone);

        entryDatetime.Text = tegucigalpaTime.ToString();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await validUserExist(_email);

    }

    private async void GoBack_Tapped(object sender, EventArgs e)
    {
        //await Navigation.PopAsync();
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {

        var data = new
        {
            email = _email,
            amount = _monto
        };

        var result = await _apiService.PostSuccessAsync("/api/user/send-money/confirmed", data);

        //await DisplayAlert(result.IsSuccess ? "Exito" : "Error", result.Message, "OK");

        var status = result.IsSuccess ? "Exito" : "Error";

        if (result.IsSuccess)
        {
            string balanceString = Preferences.Get("totBalance", "0");
            decimal balance = decimal.Parse(balanceString, CultureInfo.InvariantCulture);

            string _montoString = _monto;
            decimal xMonto = decimal.Parse(_montoString, CultureInfo.InvariantCulture);

            decimal newTotBalance = balance - xMonto;


            string newbalanceString = newTotBalance.ToString(CultureInfo.InvariantCulture);
            Preferences.Set("totBalance", newbalanceString);

            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "HomePage", ""));

        }
        else
        {
            await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
            //  await Navigation.PopAsync();
        }
    }


    private async Task validUserExist(string correo)
    {

        try
        {
            var url = ApiService.apiURL + "/api/user/send-money/exist?lang=es";

            var payload = new
            {
                email = correo
            };


            user = await LoginAsync(url, payload);

            if (user.Data != null)
            {

                Console.WriteLine($"UserExisteExito: {user.Data.exist_user.Firstname} {user.Data.exist_user.Lastname}");

                string imageUrl = user.Data.exist_user.UserImage;
                entryAvatar.Source = ImageSource.FromUri(new Uri(imageUrl));
                entryAvatar2.Source = ImageSource.FromUri(new Uri(imageUrl));

                entryName.Text = user.Data.exist_user.Firstname + " " + user.Data.exist_user.Lastname;


            }
            else
            {
                Console.WriteLine($"UserExisteError: Error");
                await DisplayAlert("User Info", "User not found", "OK");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"ErrorExiste: {ex.Message}");
        }

    }


    public async Task<ApiResponseUserExist> LoginAsync(string url, object payload)
    {
        var client = new HttpClient();
        string userToken = Preferences.Get("UserToken", string.Empty);

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken);

        var response = await client.PostAsync(url, content);

        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response UserExist: {responseBody}");


        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<ApiResponseUserExist>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return loginResponse;
        }
        else
        {
            // Manejar errores de la API
            throw new Exception("Error en la solicitud de login");
        }
    }

}