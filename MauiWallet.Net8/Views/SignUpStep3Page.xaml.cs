using System.Text.Json;
using System.Text;

namespace MauiWallet.Views;
public partial class SignUpStep3Page : ContentPage
{
    private ApiLoginResponse user;
    private const int TokenExpirationMinutes = 5;
    public SignUpStep3Page(string correo)
	{
		InitializeComponent();
        entryemail.Text = correo;

	}

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void Signin_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private async void SignUpButton_Clicked(object sender, EventArgs e)
    {

        try
        {
            var url = ApiService.apiURL + "/api/user/register?lang=es";

            var payload = new
            {
                firstname = entryfirstname.Text,
                lastname = entrylastname.Text,
                email = entryemail.Text,
                password = entrypassword.Text,
                password_confirmation = entrypassword_confirmation.Text,
                country = "Honduras",
                city = "SPS",
                phone_code = "504",
                phone = entryphone.Text,
                zip_code = "21101",
                agree = 1,
                id_device = App.DeviceId
            };

            user = await SignUpAsync(url, payload);

            if (user.Data != null)
            {

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

                //Preferences.Set("TokenExpiration", DateTime.UtcNow.AddMinutes(TokenExpirationMinutes));

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


    public async Task<ApiLoginResponse> SignUpAsync(string url, object payload)
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
            throw new Exception("Error en la solicitud de Registro");
        }
    }
}