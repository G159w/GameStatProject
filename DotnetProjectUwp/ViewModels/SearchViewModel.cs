using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CommonServiceLocator;
using DotnetProjectUwp.Models;
using DotnetProjectUwp.Models.GamesInfo;
using DotnetProjectUwp.ViewModels;
using DotnetProjectUwp.ViewModels.Games;
using DotnetProjectUwp.ViewModels.Profile;
using DotnetProjectUwp.Views.Games;
using DotNetProjectUwp.Views.Games;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Models.Responses;
using RestSharp;

namespace DotNetProjectUwp.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private readonly ViewModelNavigation _viewModelNavigation;
        private readonly RestClient _restClient;

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

        private List<UserGlobalInfo> _user;

        public List<UserGlobalInfo> Users
        {
            get => _user;
            set
            {
                _user = value;
                RaisePropertyChanged();
            }
        }
        
        public ICommand SearchCommand => new RelayCommand(async () =>
        {
            Users = null;
            var newList = new List<UserGlobalInfo>();
            var request = new RestRequest("User/search", Method.GET);
            request.AddQueryParameter("username", UserName);
            Status = "Status : Searching users ...";
            IsLoading = true;
            var result = await _restClient.ExecuteTaskAsync<IEnumerable<UserResponse>>(request);
            if (result.IsSuccessful && result.Data != null)
            {
                newList.AddRange(result.Data.Select(userResponse => new UserGlobalInfo
                {
                    Name = userResponse.Username,
                    Id = userResponse.Id,
                    Games = ProfileViewModel.ConvertProfileGame(userResponse.Games).ToList()
                }));
                Status = "Loaded";
            }
            else
            {
                Status = "Status: Failed to get user";
            }
            IsLoading = false;
            Users = newList;
        });

        public ICommand GameCommand => new RelayCommand<GameInfo>(game => LoadGame(game, _viewModelNavigation));

        public static void LoadGame(GameInfo game, ViewModelNavigation viewModelNavigation)
        {
            Console.WriteLine(game.Name);

            if (game is GameEliteInfo)
            {
                viewModelNavigation.LoadPage(new ElitePage());
                ServiceLocator.Current.GetInstance<EliteViewModel>().LoadAsync(game.Name);
            }
            else if (game is GameR6Info)
            {
                viewModelNavigation.LoadPage(new R6Page());
                ServiceLocator.Current.GetInstance<R6ViewModel>().LoadAsync(game.Name);
            }
            else if (game is GameLolInfo)
            {
                viewModelNavigation.LoadPage(new LolPage());
                ServiceLocator.Current.GetInstance<LolViewModel>().LoadAsync(game.Name);
            }
            else if (game is GameFortniteInfo)
            {
                viewModelNavigation.LoadPage(new FornitePage());
                ServiceLocator.Current.GetInstance<FortniteViewModel>().LoadAsync(game.Name);
            }
            else if (game is GameGw2Info)
            {
                viewModelNavigation.LoadPage(new Gw2Page());
                ServiceLocator.Current.GetInstance<Gw2ViewModel>().LoadAsync(game.Name);
            }
        }

        public SearchViewModel(RestClient restClient, ViewModelNavigation viewModelNavigation)
        {
            _viewModelNavigation = viewModelNavigation;
            _restClient = restClient;
        }
    }
}