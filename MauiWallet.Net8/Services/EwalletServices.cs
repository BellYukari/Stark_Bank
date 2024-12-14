
namespace MauiWallet.Services;
public class EwalletServices
{
    static EwalletServices _instance;

    public static EwalletServices Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EwalletServices();

            return _instance;
        }

    }

    public static readonly Random Random = new Random();
    public List<Color> Colors { get; } = new List<Color>()
    {
        Color.FromArgb("#7644ad"),
        Color.FromArgb("#d54381"),
        Color.FromArgb("#E88F1A"),
        Color.FromArgb("#8010E0"),
        Color.FromArgb("#7ed321"),
        Color.FromArgb("#ff4a4a"),
        Color.FromArgb("#ff844a"),
        Color.FromArgb("#4acaff"),
        Color.FromArgb("#567cd7"),
        Color.FromArgb("#523ee8"),
        Color.FromArgb("#35c659"),
        Color.FromArgb("#d483fc")
    };

    public UserInfo GetUserInfo
    {
        get
        {
            return new UserInfo
            {
                Name = "Yefrin Amaya",
                Email = "info@yefrinpacheco.com",
                Avatar = "https://starbank.yefrinpacheco.com/public/backend/images/default/profile-default.webp"
            };
        }
    }
    public List<Notification> GetNotifications
    {
        get
        {
            return new List<Notification>
            {
                new Notification
                {
                    Title = "Factura Pagada",
                    Description = "Exito",
                    Icon = "https://starbank.yefrinpacheco.com/public/backend/images/default/profile-default.webp"
                },
                new Notification
                {
                    Title = "Recarga Tigo Honduras",
                    Description = "Exito",
                    Icon = "https://starbank.yefrinpacheco.com/public/backend/images/default/profile-default.webp"
                },

            };
        }
    }


    public List<Integrante> GetIntegrantes
    {
        get
        {
            return new List<Integrante>
            {
                new Integrante
                {
                    Title = "Yefrin Benigno Amaya",
                    Description = "201710010063",
                    Icon = "https://starbank.yefrinpacheco.com/public/backend/images/default/profile-default.webp"
                },
                new Integrante
                {
                    Title = "Juan Angel Trejo",
                    Description = "201910010070",
                    Icon = "https://starbank.yefrinpacheco.com/public/backend/images/default/profile-default.webp"
                },

                new Integrante
                {
                    Title = "Carlos Alonso",
                    Description = "201720040044",
                    Icon = "https://starbank.yefrinpacheco.com/public/backend/images/default/profile-default.webp"
                },

                new Integrante
                {
                    Title = "Adolfo Carranza",
                    Description = "201020043069",
                    Icon = "https://starbank.yefrinpacheco.com/public/backend/images/default/profile-default.webp"
                },

                new Integrante
                {
                    Title = "Delcer Oviel Hernández",
                    Description = "201510050050",
                    Icon = "https://starbank.yefrinpacheco.com/public/backend/images/default/profile-default.webp"
                },

                new Integrante
                {
                    Title = "Cristian Josue Funes",
                    Description = "202110010276",
                    Icon = "https://starbank.yefrinpacheco.com/public/backend/images/default/profile-default.webp"
                },

            };
        }
    }

    public List<TransactionData> GetTransactions
    {
        get
        {
            return new List<TransactionData>
            {
 
            };
        }
    }

    public List<WalletContact> GetContacts
    {
        get
        {
            return new List<WalletContact>
            {

            };
        }
    }

    public List<ServiceCategory> GetPaymentServices
    {
        get
        {
            return new List<ServiceCategory>
            {

            };
        }
    }

    public ObservableCollection<ServiceCategoryGroup> GetAllServices
    {
        get
        {
            return new ObservableCollection<ServiceCategoryGroup>
            {
              
            };
        }
    }
    public List<CardData> GetMyCards
    {
        get
        {
            return new List<CardData>
            {
                new CardData
                {
                    CardType = "TARJETA DE DEBITO",
                    Number = "****  ****  ****  5838",
                    Name = Preferences.Get("username", null),
                    Expiry = "08/29",
                    CardCvv = 123,
                    BackgroundImage = "abs_bg.png",
                    BackgroundGradientStart = Color.FromArgb("#BF3F0041"),
                    BackgroundGradientEnd = Color.FromArgb("#012E8B"),
                    CardTypeIcon = "master_card.png"
                },
            };
        }
    }
}
