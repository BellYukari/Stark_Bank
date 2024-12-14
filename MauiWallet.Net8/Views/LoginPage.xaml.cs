using Microsoft.Maui.Controls;
using Plugin.Maui.Biometric;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Plugin.Maui.Biometric;

namespace MauiWallet.Views;

public partial class LoginPage : ContentPage
{

    private ApiLoginResponse user;
    private const int TokenExpirationMinutes = 5;

    public LoginPage()
	{
        InitializeComponent();

        string deviceId = App.DeviceId;

    }

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new DemoStartPage());
    }

 
    private async void SigninButton_Clicked(object sender, EventArgs e)
    {

        try
        {
            var email = usernameEntry.Text;
            var password = passwordEntry.Text;

            var url = ApiService.apiURL+ "/api/user/login?lang=es";

            var payload = new
            {
                email = email,
                password = password,
                id_device = App.DeviceId
            };


            user = await LoginAsync(url, payload);

            if (user.Data != null)
            {

                // Aquí puedes usar la respuesta de loginResponse
                Console.WriteLine($"Token: {user.Data.Token}");
                Console.WriteLine($"User: {user.Data.User.Firstname} {user.Data.User.Lastname}");

                Preferences.Set("username", user.Data.User.Firstname+" "+ user.Data.User.Lastname);

                // Save the token, expiration time, and user data
                Preferences.Set("UserToken", user.Data.Token);

                decimal balance = user.Data.User.Wallets[0].Balance;
                string balanceString = balance.ToString(CultureInfo.InvariantCulture);
                Preferences.Set("totBalance", balanceString);

                // Define the Central Standard Time zone (UTC-6 without daylight saving time)
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

                // Get the current UTC time and convert it to Tegucigalpa time
                DateTime utcNow = DateTime.UtcNow;
                DateTime tegucigalpaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, cstZone);

                // Calculate the token expiration time in Tegucigalpa time
                DateTime tokenExpirationTime = tegucigalpaTime.AddMinutes(TokenExpirationMinutes);

                // Store the token expiration time in preferences
                Preferences.Set("TokenExpiration", tokenExpirationTime);



               // Preferences.Set("TokenExpiration", DateTime.UtcNow.AddMinutes(TokenExpirationMinutes));

                var userDataJson = JsonSerializer.Serialize(user.Data.User);
                Preferences.Set("UserData", userDataJson);


                App.UserService.CurrentUser = user.Data.User;


                Application.Current.MainPage = new AppShell();


            }
            else
            {
                await DisplayAlert("User Info", "User not found", "OK");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }


    public async Task<ApiLoginResponse> LoginAsync(string url, object payload)
    {
        var client = new HttpClient();
      
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<ApiLoginResponse>(jsonResponse, new JsonSerializerOptions
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


    private async void ValidateBiometric_Clicked(object sender, EventArgs e)
    {
        var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
        {
            Title = "Authenticate para continuar",
            NegativeText = "Cancelar autenticacion"
        }, CancellationToken.None);

        if (result.Status == BiometricResponseStatus.Success)
        {

            var url = ApiService.apiURL + "/api/user/loginBiometric?lang=es";
            var payload = new
            {
                id_device = App.DeviceId
            };

            user = await LoginAsync(url, payload);

            if (user.Data != null)
            {

                // Aquí puedes usar la respuesta de loginResponse

                // Aquí puedes usar la respuesta de loginResponse
                Console.WriteLine($"Token: {user.Data.Token}");
                Console.WriteLine($"User: {user.Data.User.Firstname} {user.Data.User.Lastname}");

                // Save the token, expiration time, and user data
                Preferences.Set("UserToken", user.Data.Token);

                decimal balance = user.Data.User.Wallets[0].Balance;
                string balanceString = balance.ToString(CultureInfo.InvariantCulture);
                Preferences.Set("totBalance", balanceString);

                // Define the Central Standard Time zone (UTC-6 without daylight saving time)
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

                // Get the current UTC time and convert it to Tegucigalpa time
                DateTime utcNow = DateTime.UtcNow;
                DateTime tegucigalpaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, cstZone);

                // Calculate the token expiration time in Tegucigalpa time
                DateTime tokenExpirationTime = tegucigalpaTime.AddMinutes(TokenExpirationMinutes);

                // Store the token expiration time in preferences
                Preferences.Set("TokenExpiration", tokenExpirationTime);



                // Preferences.Set("TokenExpiration", DateTime.UtcNow.AddMinutes(TokenExpirationMinutes));

                var userDataJson = JsonSerializer.Serialize(user.Data.User);
                Preferences.Set("UserData", userDataJson);


                App.UserService.CurrentUser = user.Data.User;


                Application.Current.MainPage = new AppShell();


            }
            else
            {
                await DisplayAlert("User Info", "User not found", "OK");
            }
        }
        else
        {
            await DisplayAlert("Nope", "Couldnt authenticate", "OK");
        }
    }


    private async void ForgotPassword_Tapped(object sender, TappedEventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage());
    }

    private async void ExchangeRate_Tapped(object sender, TappedEventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new ExchangeRatePage());
    }

    private async void Signup_Tapped(object sender, TappedEventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new SignUpStep1Page());
    }


}