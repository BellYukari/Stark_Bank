using ZXing.Net.Maui;

namespace MauiWallet.Views;

public partial class ScanQrPayPage : ContentPage
{
    private bool _isAlertShown = false;
    private ApiService _apiService;
    public ScanQrPayPage()
	{
		InitializeComponent();
        _apiService = new ApiService();

        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = true
        };
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {

        if (_isAlertShown)
            return;

        _isAlertShown = true;
        var barcodeValue = e.Results[0].Value;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            //_isAlertShown = false;

            var data = new
            {
                email = barcodeValue
            };

            var result = await _apiService.PostSuccessAsync("/api/user/send-money/exist", data);
            var status = result.IsSuccess ? "Exito" : "Error";

            if (result.IsSuccess)
            {
                await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "TransferMoneyStep2Page", barcodeValue));
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
            }


        });
    }

    void SwitchCameraButton_Clicked(object sender, EventArgs e)
    {
        barcodeView.CameraLocation = barcodeView.CameraLocation == CameraLocation.Rear ? CameraLocation.Front : CameraLocation.Rear;
    }

    void TorchButton_Clicked(object sender, EventArgs e)
    {
        barcodeView.IsTorchOn = !barcodeView.IsTorchOn;
    }

    private async void OnClose_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}