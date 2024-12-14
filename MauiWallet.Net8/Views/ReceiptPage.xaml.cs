using SkiaSharp;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace MauiWallet.Views;

public partial class ReceiptPage : ContentPage
{
	public ReceiptPage( string trxValue, string amountValue, string date_timeValue)
	{
		InitializeComponent();
        entryLabelNombre.Text = trxValue;
        entryMonto.Text = amountValue;
        entryDatetime.Text = date_timeValue;

    }

    private async void GoBack_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }


    public async Task<byte[]> CaptureViewAsImageAsync(VisualElement view)
    {
        if (view == null)
        {
            throw new ArgumentNullException(nameof(view), "La vista no puede ser nula.");
        }

        await Task.Delay(100);

        var width = (int)view.Width;
        var height = (int)view.Height;

        if (width <= 0 || height <= 0)
        {
            throw new InvalidOperationException("El tamaño de la vista no es válido.");
        }

        using (var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul)))
        {
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);


            var image = surface.Snapshot();
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = new MemoryStream())
            {
                data.SaveTo(stream);
                return stream.ToArray();
            }
        }
    }

    public string SaveImageToFile(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0)
        {
            throw new ArgumentException("Los datos de la imagen no pueden ser nulos o vacíos.", nameof(imageData));
        }

        var fileName = Path.Combine(FileSystem.CacheDirectory, "capturedImage.png");
        File.WriteAllBytes(fileName, imageData);
        return fileName;
    }

    public async Task ShareImageAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            throw new FileNotFoundException("El archivo para compartir no se encuentra.", filePath);
        }

        await Share.RequestAsync(new ShareFileRequest
        {
            Title = "Compartir imagen",
            File = new ShareFile(filePath)
        });
    }

    public async void OnShareButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var verticalStackLayout = this.FindByName<VerticalStackLayout>("CaptureArea");
            var imageBytes = await CaptureViewAsImageAsync(verticalStackLayout);
            var filePath = SaveImageToFile(imageBytes);
            await ShareImageAsync(filePath);
        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }




}