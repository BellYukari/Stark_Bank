namespace MauiWallet.ViewModels;
public partial class AllServiceViewModel : ObservableObject
{
    public AllServiceViewModel()
    {
        LoadData();
    }

    void LoadData()
    {
        AllServices = new ObservableCollection<ServiceCategoryGroup>(EwalletServices.Instance.GetAllServices);
    }

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    public ObservableCollection<ServiceCategoryGroup> _allServices;
}
