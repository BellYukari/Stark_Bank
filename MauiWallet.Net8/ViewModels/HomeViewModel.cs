
namespace MauiWallet.ViewModels;
public partial class HomeViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private ObservableCollection<TransactionData> _recentTransactions;

    [ObservableProperty]
    UserInfo _userInfo;

    public HomeViewModel()
    {
        LoadData();
    }

    void LoadData()
    {
        UserInfo = EwalletServices.Instance.GetUserInfo;
        RecentTransactions = new ObservableCollection<TransactionData>(EwalletServices.Instance.GetTransactions);
    }

    [RelayCommand]
    private async void GotoTopup()
    {
        await Shell.Current.GoToAsync(nameof(MobileTopupPage));
    }

    [RelayCommand]
    private async void GotoTransfer()
    {
        await Shell.Current.GoToAsync(nameof(TransferMoneyStep1Page));
    }

    [RelayCommand]
    private async void GotoRequest()
    {
        await Shell.Current.GoToAsync(nameof(ReceiveMoneyPage));
    }

    [RelayCommand]
    private async void About()
    {
        await PopupNavigation.Instance.PushAsync(new AboutPage());
    }
}
