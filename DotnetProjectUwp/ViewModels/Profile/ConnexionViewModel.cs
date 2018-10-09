using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DotnetProjectUwp.Views.Profile;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Profile
{
    public class ConnexionViewModel : ViewModelBase
    {
        private readonly ViewModelNavigation _viewModelNavigation;

        private readonly RestClient _restClient;

        public ICommand ConnexionCommand => new RelayCommand(() => Connexion());

        public ICommand InscriptionCommand => new RelayCommand(Inscription);

        private string _userName;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChanged();
            }
        }

        private string _status;

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

        public string Password { get; set; }

        public async Task Connexion()
        {
            var request = new RestRequest("User/login", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(new { username = UserName, password = Password });
            IsLoading = true;
            Status = "Status: loading ...";
            var response = await _restClient.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var responseObject = JObject.Parse(response.Content);
                var jwt = responseObject["token"].ToString();

                IList<Claim> claimCollection = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, UserName)
                    , new Claim("jwt", jwt)
                };

                var identity = new ClaimsIdentity(claimCollection, ClaimTypes.Name);
                var principal = new ClaimsPrincipal(identity);
                Thread.CurrentPrincipal = principal;
                ClaimsPrincipal.ClaimsPrincipalSelector = () => principal;
                _viewModelNavigation.LoadPage(new ProfilePage());
                _viewModelNavigation.NewContext();
            }
            else
            {
                Status = "Status: invalid username / password";
            }

            IsLoading = false;
        }

        public void Inscription()
        {
            _viewModelNavigation.LoadPage(new InscriptionPage());
        }

        public ConnexionViewModel(ViewModelNavigation viewModelNavigation, RestClient restClient)
        {
            _viewModelNavigation = viewModelNavigation;
            _restClient = restClient;
        }
    }
}
