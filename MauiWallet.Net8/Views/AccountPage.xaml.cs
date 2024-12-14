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
            // Mostrar una alerta de confirmación antes de cerrar sesión
            bool confirmLogout = await DisplayAlert("Confirmación", "¿Deseas cerrar sesión?", "Sí", "No");

            if (!confirmLogout)
            {
                // Si el usuario elige "No", simplemente salimos del método
                return;
            }

            // Obtener el token del usuario desde las preferencias
            var UserToken = Preferences.Get("UserToken", null);

            // Configurar la URL para la solicitud de cierre de sesión
            string url = ApiService.apiURL + "/api/user/logout?lang=es";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserToken);

            // Enviar la solicitud de cierre de sesión
            var response = await _httpClient.GetStringAsync(url);

            // Mostrar un mensaje de éxito
            await DisplayAlert("Éxito", "Cerraste sesión con éxito", "OK");

            // Limpiar las preferencias del usuario y el estado de la aplicación
            Preferences.Remove("UserToken");
            Preferences.Remove("TokenExpiration");
            Preferences.Remove("UserData");
            Preferences.Remove("totBalance");
            Preferences.Remove("username");
            App.UserService.CurrentUser = null;

            // Limpiar la pila de navegación
            //await Navigation.PopToRootAsync();

            // Navegar a la página de inicio de sesión
            //await Navigation.PushAsync(new LoginStyle1Page()); // El parámetro 'false' evita animaciones

            Application.Current.MainPage = new NavigationPage(new LoginPage());

            //Application.Current.MainPage = new LoginPage();
            //await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());

            // Opcional: Desactivar el botón de retroceso en la página de inicio de sesión
            // Esto puede ser útil si tu aplicación utiliza una navegación personalizada
            // Puedes manejar esto dentro de la propia página de inicio de sesión
        }
        catch (Exception ex)
        {
            // Manejar excepciones y mostrar el mensaje de error
            Console.WriteLine("Error: " + ex.Message);
            await DisplayAlert("Error", "Ocurrió un error al intentar cerrar sesión. Inténtalo de nuevo.", "OK");
        }
    }






}