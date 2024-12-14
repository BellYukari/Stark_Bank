using System.Text.Json;
using MauiWallet.Services;

using Microsoft.Maui.Graphics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using SkiaSharp;


namespace MauiWallet.Views;
public partial class ReceiveMoneyPage : ContentPage
{
    private ApiService _apiService;
    private readonly HttpClient _httpClient;
    private string qrImageUrl;
    public ReceiveMoneyPage()
	{
		InitializeComponent();
        _apiService = new ApiService();
        _httpClient = new HttpClient();

        GetDataAsync();
    }

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnShareButtonClicked(object sender, EventArgs e)
    {
        var imageStream = await CaptureImage(entryQR);

        if (imageStream != null)
        {
            var filePath = Path.Combine(FileSystem.CacheDirectory, "shared_image.png");
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await imageStream.CopyToAsync(fileStream);
            }

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Compartir Imagen",
                File = new ShareFile(filePath)
            });
        }
    }

    private async Task<Stream> CaptureImage(Image image)
    {
        // Descargar la imagen desde la URL (simula la captura del componente Image)
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(qrImageUrl);
            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                return new MemoryStream(imageBytes);
            }
            return null;
        }
    }


    public async Task GetDataAsync()
    {

        var UserToken = Preferences.Get("UserToken", null);
        string url = ApiService.apiURL + "/api/user/receive-money?lang=es";

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserToken);

        var response = await _httpClient.GetStringAsync(url);
        Console.WriteLine($"CodigoQR: {response}");

        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });



        //ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(response);

        // Accede a los datos
        string uniqueCode = apiResponse.Data.UniqueCode;
        string qrCode = apiResponse.Data.QrCode;
        qrImageUrl = qrCode;

        Console.WriteLine($"CodigoQR2: {qrCode}");

        entryCorreo.Text = uniqueCode;
        entryQR.Source = ImageSource.FromUri(new Uri(qrCode));

    }

    // Modelo para el objeto de mensaje
    public class Message
    {
        public List<string> Success { get; set; }
    }

    // Modelo para el objeto de datos
    public class Data
    {
        public string UniqueCode { get; set; }
        public string QrCode { get; set; }
    }

    // Modelo para la respuesta completa de la API
    public class ApiResponse
    {
        public Message Message { get; set; }
        public Data Data { get; set; }
    }


}