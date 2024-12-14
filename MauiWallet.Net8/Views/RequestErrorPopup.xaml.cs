using Microsoft.Maui.ApplicationModel.Communication;

namespace MauiWallet.Views;
public partial class RequestErrorPopup : PopupPage
{
    public RequestErrorPopup(string title, string descri)
    {
        InitializeComponent();
        entrytitle.Text = title;
        entrydescri.Text = descri;
    }

    private async void AceptarButton_Clicked(object sender, EventArgs e)
    {
        await PopupNavigation.Instance.PopAsync();
    }

}