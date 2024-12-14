using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiWallet.Models;

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Maui.Controls.Shapes;

namespace MauiWallet.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public static string apiURL = "https://neovista.xyz";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }


        public async Task<T> GetDataAsync<T>(string endpoint)
        {
            try
            {
                string url = $"{apiURL}{endpoint}?lang=es";
                var response = await _httpClient.GetStringAsync(url);
                var transactionsResponse = JsonSerializer.Deserialize<ApiResponseTransactions>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return JsonSerializer.Deserialize<T>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return default;
            }
        }

        public async Task<TResponse> PostDataAsync<TResponse>(string endpoint, object data)
        {
            try
            {
                string url = $"{apiURL}{endpoint}?lang=es";
                var jsonData = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostDataAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<ApiResponseResultR1> PostSuccessAsync(string endpoint, object data)
        {
            try
            {
                var UserToken = Preferences.Get("UserToken", null);

                string url = $"{apiURL}{endpoint}?lang=es";
                Console.WriteLine($"RutaURL: {url}");

                var jsonData = JsonSerializer.Serialize(data);
                Console.WriteLine($"Data being sent: {jsonData}");

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserToken);

                var response = await _httpClient.PostAsync(url, content);
                Console.WriteLine($"Response status code: {response.StatusCode}");

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response body: {responseBody}");

                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody);

                if (response.IsSuccessStatusCode)
                {

                    string errorMessage = "Success.";

                    using (JsonDocument doc = JsonDocument.Parse(responseBody))
                    {
                        if (doc.RootElement.TryGetProperty("message", out JsonElement messageElement) &&
                            messageElement.TryGetProperty("success", out JsonElement errorElement) &&
                            errorElement.ValueKind == JsonValueKind.Array &&
                            errorElement.GetArrayLength() > 0)
                        {
                            errorMessage = errorElement[0].GetString();
                        }
                    }

                    return new ApiResponseResultR1 { IsSuccess = true, Message = errorMessage };

                }
                else
                {
                    string errorMessage = "Error.";

                    using (JsonDocument doc = JsonDocument.Parse(responseBody))
                    {
                        if (doc.RootElement.TryGetProperty("message", out JsonElement messageElement) &&
                            messageElement.TryGetProperty("error", out JsonElement errorElement) &&
                            errorElement.ValueKind == JsonValueKind.Array &&
                            errorElement.GetArrayLength() > 0)
                        {
                            errorMessage = errorElement[0].GetString();
                        }
                    }

                    return new ApiResponseResultR1 { IsSuccess = false, Message = errorMessage };

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostSuccessAsync: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return new ApiResponseResultR1 { IsSuccess = false, Message = ex.Message };
            }
        }








        public async Task<bool> DeleteDataAsync(string endpoint, int id)
        {
            try
            {
                string url = $"{apiURL}{endpoint}/{id}";
                Console.WriteLine($"RutaDetele: {url}");


                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error in DeleteDataAsync: {ex.Message}");
                return false;
            }
        }



        public async Task<bool> UpdateDataAsync(string endpoint, int id, object data)
        {
            try
            {
                string url = $"{apiURL}{endpoint}/{id}";
                Console.WriteLine($"RutaURL: {url}");

                var jsonData = JsonSerializer.Serialize(data);
                Console.WriteLine($"Data being sent: {jsonData}");

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await PatchAsync(url, content);
                Console.WriteLine($"Response status code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response body: {responseBody}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostSuccessAsync: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }

        private async Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };

            return await _httpClient.SendAsync(request);
        }


        public async Task<bool> UpdateDataAsync2(string endpoint, int id, object data)
        {
            try
            {
                string url = $"{apiURL}{endpoint}/{id}";
                var jsonData = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();

                // Si la respuesta es buena, regresa true
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateDataAsync: {ex.Message}");
                return false;
            }
        }
    }
}
