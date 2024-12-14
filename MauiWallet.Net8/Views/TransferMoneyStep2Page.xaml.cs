using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;



using Microsoft.Maui.Controls;

using System;
using System.Collections.Generic;



namespace MauiWallet.Views;

public partial class TransferMoneyStep2Page : ContentPage
{
    private ApiResponseUserExist user;
    private string _email;
    public TransferMoneyStep2Page(string email)
	{
		InitializeComponent();
        //BindingContext = new PaymentConfirmViewModel();
        _email = email;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await validUserExist(_email);

    }

    private void ContinueButton_Clicked(object sender, EventArgs e)
    {
        // Navigation.PushAsync(new TransferPaymentConfirmPage(_email, entryMonto.Text, entryNota.Text));
        Application.Current.MainPage = new TransferPaymentConfirmPage(_email, entryMonto.Text, entryNota.Text);

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

                entryName.Text = user.Data.exist_user.Firstname+" "+ user.Data.exist_user.Lastname;

                string balanceString = Preferences.Get("totBalance", "0");
                decimal balance = decimal.Parse(balanceString, CultureInfo.InvariantCulture);
                entryBalance.Text = "L. " + balance.ToString("F2");

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