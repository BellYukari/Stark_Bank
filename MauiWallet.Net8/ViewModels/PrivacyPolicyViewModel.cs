
namespace MauiWallet.ViewModels;
public partial class PrivacyPolicyViewModel : ObservableObject
{
    [ObservableProperty]
    string _url;
    public PrivacyPolicyViewModel()
    {
        Url = "https://yefrinpacheco.com/politica.html";
    }
}
