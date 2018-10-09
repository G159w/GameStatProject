using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DotnetProjectUwp.Models;
using DotnetProjectUwp.Models.GamesInfo;
using DotNetProjectUwp.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Leaderboard
{
    public abstract class LeaderboardViewModel<T> : ViewModelBase
    {
        private readonly RestClient _restClient;

        protected readonly ViewModelNavigation ViewModelNavigation;

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

        public ICommand GameUserCommand => new RelayCommand<GameInfo>(game => SearchViewModel.LoadGame(game, ViewModelNavigation));

        public ICommand ReloadCommand => new RelayCommand(() => LoadAsync(CreateRestRequest()));

        private IEnumerable<T> _leaderboardResponse;

        public IEnumerable<T> LeaderboardResponse
        {
            get => _leaderboardResponse;
            set
            {
                _leaderboardResponse = value;
                RaisePropertyChanged();
            }
        }

        private IEnumerable<UserRank> _userRanks;

        public IEnumerable<UserRank> UserRanks
        {
            get => _userRanks;
            set
            {
                _userRanks = value;
                RaisePropertyChanged();
            }
        }

        private string _selectedOption;

        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                RaisePropertyChanged();
            }
        }

        private List<string> _sortOptions;

        public List<string> SortOptions
        {
            get => _sortOptions;
            set
            {
                _sortOptions = value;
                RaisePropertyChanged();
            }
        }

        protected LeaderboardViewModel(RestClient restClient, ViewModelNavigation viewModelNavigation)
        {
            _restClient = restClient;
            ViewModelNavigation = viewModelNavigation;
        }

        public async Task LoadAsync(RestRequest request)
        {
            LeaderboardResponse = new List<T>();
            Status = "Satus: Getting user Information ...";
            IsLoading = true;
            var result = await _restClient.ExecuteTaskAsync<IEnumerable<T>>(request);

            if (result.IsSuccessful)
            {
                Status = "Status: Loaded";
                LeaderboardResponse = result.Data;
                TransformLeaderboardResponse();
            }
            else
            {
                Status = "Status: Failed to get user datas";
            }

            IsLoading = false;
        }

        public abstract void TransformLeaderboardResponse();

        public abstract RestRequest CreateRestRequest();
    }
}
