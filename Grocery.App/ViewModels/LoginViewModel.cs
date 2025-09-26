
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;


namespace Grocery.App.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly IClientService _clientService;
        private readonly GlobalViewModel _global;

        [ObservableProperty]
        private bool loginVisible = true;

        [ObservableProperty]
        private bool createVisible = false;

        [ObservableProperty]
        private string email = "user3@mail.com";

        [ObservableProperty]
        private string password = "user3";

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string createMessage;

        [ObservableProperty]
        private string loginMessage;

        public LoginViewModel(IAuthService authService, GlobalViewModel global)
        { //_authService = App.Services.GetServices<IAuthService>().FirstOrDefault();
            _authService = authService;
            _global = global;
        }

        [RelayCommand]
        private void Create()
        {
            email = "";
            password = "";
            LoginVisible = false;
            CreateVisible = true;
        }
        
        [RelayCommand]
        private void LoginAfterCreate()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                CreateMessage = "Vul alle velden in.";
                return;
            }

            Client? newClient = _clientService.Create(Name, Email, Password);

            if (newClient != null)
            {
                CreateMessage = $"Account aangemaakt! Welkom {newClient.Name}.";
                LoginVisible = true;
                CreateVisible = false;
                _global.Client = newClient;
                // Application.Current.MainPage = new AppShell();
                Console.WriteLine($"welkom {newClient}");
            }
            else
            {
                CreateMessage = "Email bestaat al.";
            }
        }
        
        
        [RelayCommand]
        private void Login()
        {
            Client? authenticatedClient = _authService.Login(Email, Password);
            if (authenticatedClient != null)
            {
                LoginMessage = $"Welkom {authenticatedClient.Name}!";
                _global.Client = authenticatedClient;
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                LoginMessage = "Ongeldige inloggegevens.";
            }
        }
    }
}
