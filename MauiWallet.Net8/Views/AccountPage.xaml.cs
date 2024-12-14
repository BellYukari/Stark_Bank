namespace MauiWallet.Views;

public partial class AccountPage : ContentPage
{
    private ApiService _apiService;
    private readonly HttpClient _httpClient;
    public AccountPage()
    {
        InitializeComponent();
        BindingContext = new AccountViewModel();
        _apiService = new ApiService();
        _httpClient = new HttpClient();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Acceder al usuario guardado en el servicio
        var currentUser = App.UserService.CurrentUser;

        if (currentUser != null)
        {
            // Mostrar el Firstname del usuario
            entryUsername.Text = Preferences.Get("username", null);
            entryEmail.Text = currentUser.Email;

            string imageUrl = currentUser.UserImage;
            entryAvatar.Source = ImageSource.FromUri(new Uri(imageUrl));
        }
    }

    private async void Logout_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            // Mostrar una alerta de confirmaci�n antes de cerrar sesi�n
            bool confirmLogout = await DisplayAlert("Confirmaci�n", "�Deseas cerrar sesi�n?", "S�", "No");

            if (!confirmLogout)
            {
                // Si el usuario elige "No", simplemente salimos del m�todo
                return;
            }

            // Obtener el token del usuario desde las preferencias
            var UserToken = Preferences.Get("UserToken", null);

            // Configurar la URL para la solicitud de cierre de sesi�n
            string url = ApiService.apiURL + "/api/user/logout?lang=es";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserToken);

            // Enviar la solicitud de cierre de sesi�n
            var response = await _httpClient.GetStringAsync(url);

            // Mostrar un mensaje de �xito
            await DisplayAlert("�xito", "Cerraste sesi�n con �xito", "OK");

            // Limpiar las preferencias del usuario y el estado de la aplicaci�n
            Preferences.Remove("UserToken");
            Preferences.Remove("TokenExpiration");
            Preferences.Remove("UserData");
            Preferences.Remove("totBalance");
            Preferences.Remove("username");
            App.UserService.CurrentUser = null;

            // Limpiar la pila de navegaci�n
            //await Navigation.PopToRootAsync();

            // Navegar a la p�gina de inicio de sesi�n
            //await Navigation.PushAsync(new LoginStyle1Page()); // El par�metro 'false' evita animaciones

            Application.Current.MainPage = new NavigationPage(new LoginPage());

            //Application.Current.MainPage = new LoginPage();
            //await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());

            // Opcional: Desactivar el bot�n de retroceso en la p�gina de inicio de sesi�n
            // Esto puede ser �til si tu aplicaci�n utiliza una navegaci�n personalizada
            // Puedes manejar esto dentro de la propia p�gina de inicio de sesi�n
        }
        catch (Exception ex)
        {
            // Manejar excepciones y mostrar el mensaje de error
            Console.WriteLine("Error: " + ex.Message);
            await DisplayAlert("Error", "Ocurri� un error al intentar cerrar sesi�n. Int�ntalo de nuevo.", "OK");
        }
    }






}