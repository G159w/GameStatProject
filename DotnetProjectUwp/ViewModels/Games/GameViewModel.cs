using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Games
{
    public abstract class GameViewModel<T> : ViewModelBase
    {
        private readonly string _game;

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

        private T _statsResponse;

        public T StatsResponse
        {
            get => _statsResponse;
            set
            {
                _statsResponse = value;
                RaisePropertyChanged();
            }
        }

        protected readonly RestClient RestClient;

        protected GameViewModel(RestClient restClient, string game)
        {
            RestClient = restClient;
            _game = game;
        }

        public async Task LoadAsync(string userName)
        {
            UserName = userName;
            StatsResponse = default(T);
            var request = new RestRequest($"{_game}/{userName}");
            Status = "Satus: Getting user Information ...";
            IsLoading = true;
            var result = await RestClient.ExecuteTaskAsync<T>(request);

            if (result.IsSuccessful)
            {
                Status = "Status: Loaded";
                StatsResponse = result.Data;
            }
            else
            {
                Status = "Status: Failed to get user datas";
            }

            IsLoading = false;
        }
    }
}
