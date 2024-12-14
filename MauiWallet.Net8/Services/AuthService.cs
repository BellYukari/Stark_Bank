using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace MauiWallet.Services
{
    public class AuthService
    {
        public async Task<string> AuthenticateUser(string email, string password)
        {
            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("password", password)
            });

            var response = await httpClient.PostAsync("https://starbank.yefrinpacheco.com/api/user/login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiLoginResponse>(responseContent);
                if (apiResponse != null && apiResponse.Data != null)
                {
                    // Guardar el usuario y el token
                    await SecureStorage.SetAsync("auth_token", apiResponse.Data.Token);
                    await SecureStorage.SetAsync("user_data", JsonSerializer.Serialize(apiResponse.Data.User));

                    return apiResponse.Data.Token;
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }

            return null;
        }

        public async Task<User> GetUserDataAsync()
        {
            var userDataJson = await SecureStorage.GetAsync("user_data");
            if (!string.IsNullOrEmpty(userDataJson))
            {
                return JsonSerializer.Deserialize<User>(userDataJson);
            }
            return null;
        }

        public async Task<bool> IsTokenValidAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            var expirationTimeString = await SecureStorage.GetAsync("token_expiration");

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(expirationTimeString))
            {
                var expirationTime = DateTime.Parse(expirationTimeString, null, System.Globalization.DateTimeStyles.RoundtripKind);
                if (DateTime.UtcNow < expirationTime)
                {
                    return true;
                }
                else
                {
                    // Eliminar el token expirado
                    SecureStorage.Remove("auth_token");
                    SecureStorage.Remove("token_expiration");
                }
            }

            return false;
        }
    }
}
