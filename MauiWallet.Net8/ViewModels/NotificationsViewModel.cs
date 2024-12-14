
namespace MauiWallet.ViewModels;
public partial class NotificationsViewModel : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private ObservableCollection<Notification> _notifications;
    #endregion Properties

    #region Constructor
    public NotificationsViewModel()
    {
        LoadData();
    }
    #endregion

    #region Methods
    void LoadData()
    {
        Notifications = new ObservableCollection<Notification>(EwalletServices.Instance.GetNotifications);
    }
    #endregion Methods
}
