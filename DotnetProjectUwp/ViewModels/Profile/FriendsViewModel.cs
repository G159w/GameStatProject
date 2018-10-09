using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DotnetProjectUwp.Models.GamesInfo;
using DotNetProjectUwp.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Profile
{
    public class FriendsViewModel : ViewModelBase
    {
        private readonly RestClient _restClient;

        private readonly ViewModelNavigation _viewModelNavigation;
     
        private UserResponse _userResponse;

        public UserResponse UserResponse
        {
            get => _userResponse;
            set
            {
                _userResponse = value;
                RaisePropertyChanged();
            }
        }

        public ICommand GameCommand =>
            new RelayCommand<GameInfo>(game => SearchViewModel.LoadGame(game, _viewModelNavigation));

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

        private List<GameInfo> _ownedGames;

        public List<GameInfo> OwnedGames
        {
            get => _ownedGames;
            set
            {
                _ownedGames = value;
                RaisePropertyChanged();
            }
        }

   
        public FriendsViewModel(RestClient restClient, ViewModelNavigation viewModelNavigation)
        {
            _restClient = restClient;
            _viewModelNavigation = viewModelNavigation;
        }

        public async Task LoadUser(string userName)
        {
            OwnedGames = null;
            if (userName == null)
            {
                Status = "Status: Failed to get user Informations";
                return;
            }

            IsLoading = true;
            Status = "Status: Getting user Informations ...";
            var request = new RestRequest($"User/{userName}");
            var result = await _restClient.ExecuteTaskAsync<UserResponse>(request);
            if (result.IsSuccessful)
            {
                UserResponse = result.Data;
                Status = $"Status: {UserResponse.Username}'s informations loaded";
                OwnedGames = ProfileViewModel.ConvertProfileGame(UserResponse.Games).ToList();
            }
            else
            {
                Status = "Status: Failed to get user Informations";
            }

            IsLoading = false;
        }
    }
}
