
namespace MauiWallet.ViewModels;
public partial class AboutViewModel : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private ObservableCollection<Integrante> _integrantes;
    #endregion Properties

    #region Constructor
    public AboutViewModel()
    {
        LoadData();
    }
    #endregion

    #region Methods
    void LoadData()
    {
        Integrantes = new ObservableCollection<Integrante>(EwalletServices.Instance.GetIntegrantes);
    }
    #endregion Methods
}
