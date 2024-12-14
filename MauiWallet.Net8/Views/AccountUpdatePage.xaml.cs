namespace MauiWallet.Views;
public partial class AccountUpdatePage : PopupPage
{
    private ApiService _apiService;
    public AccountUpdatePage()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Acceder al usuario guardado en el servicio
        var currentUser = App.UserService.CurrentUser;

        if (currentUser != null)
        {
            // Mostrar el Firstname del usuario
            //entryUsername.Text = currentUser.Firstname + " " + currentUser.Lastname;
            //entryEmail.Text = currentUser.Email;

            string imageUrl = currentUser.UserImage;
            entryAvatar.Source = ImageSource.FromUri(new Uri(imageUrl));

            entryFirstname.Text = currentUser.Firstname;
            entryLastname.Text = currentUser.Lastname;
            entryCountry.Text = currentUser.Address.Country;
            entryPhone.Text = currentUser.Mobile;
            entryState.Text = currentUser.Address.State;
            entryCity.Text = currentUser.Address.City;
            entryZip_code.Text = currentUser.Address.Zip;
            entryAddress.Text = currentUser.Address.AddressLine;

        }
    }

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {
        var data = new
        {
            firstname = entryFirstname.Text,
            lastname = entryLastname.Text,
            country = entryCountry.Text,
            phone_code = "504",
            phone = entryPhone.Text,
            state = entryState.Text,
            city = entryCity.Text,
            zip_code = entryZip_code.Text,
            address = entryAddress.Text,
        };

        var result = await _apiService.PostSuccessAsync("/api/user/profile/update", data);
        var status = result.IsSuccess ? "Exito" : "Error";

        if (result.IsSuccess)
        {
            Preferences.Set("username", entryFirstname.Text+" "+ entryLastname.Text);
            await PopupNavigation.Instance.PopAsync();

            await PopupNavigation.Instance.PushAsync(new RequestPopup(status, result.Message, "AccountPage", ""));
        }
        else
        {
            await PopupNavigation.Instance.PushAsync(new RequestErrorPopup(status, result.Message));
        }
    }
}
