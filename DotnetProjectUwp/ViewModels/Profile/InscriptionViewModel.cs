using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
    public class InscriptionViewModel : ViewModelBase
    {
        private readonly ViewModelNavigation _viewModelNavigation;

        private readonly RestClient _restClient;

        public ICommand InscriptionCommand => new RelayCommand(() => Inscription());

        private string _mail;

        public string Mail
        {
            get => _mail;
            set
            {
                _mail = value;
                RaisePropertyChanged();
            }
        }

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


        public async Task Inscription()
        {
            var request = new RestRequest("User/register", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(new { email = Mail, username = UserName, password = Password });
            Status = "Status: Loading ...";
            IsLoading = true;
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

            IsLoading = false;
        }

        public InscriptionViewModel(ViewModelNavigation viewModelNavigation, RestClient restClient)
        {
            _viewModelNavigation = viewModelNavigation;
            _restClient = restClient;
        }
    }
}
