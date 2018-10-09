using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonServiceLocator;
using DotnetProjectUwp.Models.GamesInfo;
using DotnetProjectUwp.Views.Profile;
using DotNetProjectUwp.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Models.Requests;
using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Profile
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly RestClient _restClient;

        private readonly ViewModelNavigation _viewModelNavigation;

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

        private List<GameInfo> _games;

        public List<GameInfo> Games
        {
            get => _games;
            set
            {
                _games = value;
                RaisePropertyChanged();
            }
        }

        private GameInfo _selectedGameInfo;
        public GameInfo SelectedGameInfo
        {
            get => _selectedGameInfo;
            set
            {
                _selectedGameInfo = value;
                RaisePropertyChanged();
            }
        }

        private string _friendName;

        public string FriendName
        {
            get => _friendName;
            set
            {
                _friendName = value;
                RaisePropertyChanged();
            }
        }

        private GameAddRequest _gameAddRequest;

        public GameAddRequest GameAddRequest
        {
            get => _gameAddRequest;
            set
            {
                _gameAddRequest = value;
                RaisePropertyChanged();
            }
        }

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

        public ICommand FriendCommand =>
            new RelayCommand<FriendResponse>(friend =>
            {
                _viewModelNavigation.LoadPage(new FriendsPage());
                var friendsViewModel = ServiceLocator.Current.GetInstance<FriendsViewModel>();
                friendsViewModel.LoadUser(friend.Username);
            });

        public ICommand AddGameCommand =>
            new RelayCommand(async () =>
            {
                if (string.IsNullOrWhiteSpace(GameAddRequest.Username))
                {
                    Status = "Status: Invalid User Game ...";
                    return;
                }
                Status = "Status: Getting user Informations ...";
                IsLoading = true;
                var request = new RestRequest($"Game/{SelectedGameInfo.ShortNameGame}", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddBody(GameAddRequest);
                request.AddHeader("Authorization", "Bearer " + ClaimsPrincipal.Current.Claims.FirstOrDefault(_ => _.Type == "jwt")?.Value);
                var result = await _restClient.ExecuteTaskAsync(request);

                if (result.IsSuccessful)
                {
                    Status = "Status: Game Added";
                    LoadCurrentUser();
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _viewModelNavigation.LoadPage(new ConnexionPage());
                    _viewModelNavigation.NewContext();
                }
                else
                {
                    Status = "Status: Fail to add game";
                }

                IsLoading = false;
            });

        public ICommand DeleteGameCommand =>
            new RelayCommand<GameInfo>(async (game) =>
            {
                Status = "Status: Getting user Informations ...";
                IsLoading = true;
                var request = new RestRequest($"Game/{game.ShortNameGame}/{game.Name}", Method.DELETE)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddHeader("Authorization", "Bearer " + ClaimsPrincipal.Current.Claims.FirstOrDefault(_ => _.Type == "jwt")?.Value);
                var result = await _restClient.ExecuteTaskAsync(request);

                if (result.IsSuccessful)
                {
                    Status = "Status: Game Deleted";
                    LoadCurrentUser();
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _viewModelNavigation.LoadPage(new ConnexionPage());
                    _viewModelNavigation.NewContext();
                }
                else
                {
                    Status = "Status: Fail to delete game";
                }
                IsLoading = false;
            });

        public ICommand AddFriendCommand => new RelayCommand(async () =>
        {
            if (string.IsNullOrWhiteSpace(FriendName))
            {
                Status = "Status: Invalid User Game ...";
                return;
            }
            IsLoading = true;
            Status = "Status: Getting user Informations ...";
            var request = new RestRequest($"User/friends", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(new {username = FriendName});
            request.AddHeader("Authorization",
                "Bearer " + ClaimsPrincipal.Current.Claims.FirstOrDefault(_ => _.Type == "jwt")?.Value);
            var result = await _restClient.ExecuteTaskAsync(request);

            if (result.IsSuccessful)
            {
                Status = "Status: Friend added";
                LoadCurrentUser();
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                _viewModelNavigation.LoadPage(new ConnexionPage());
                _viewModelNavigation.NewContext();
            }
            else
            {
                Status = "Status: Fail to add friend";
            }
            IsLoading = false;
        });

        public ICommand DeleteFriendCommand => new RelayCommand<FriendResponse>(async (friend) =>
        {
            Status = "Status: Getting user Informations ...";
            IsLoading = true;
            var request = new RestRequest($"User/friends/{friend.Username}", Method.DELETE);
            request.AddHeader("Authorization",
                "Bearer " + ClaimsPrincipal.Current.Claims.FirstOrDefault(_ => _.Type == "jwt")?.Value);
            var result = await _restClient.ExecuteTaskAsync(request);

            if (result.IsSuccessful)
            {
                Status = "Status: Friend deleted";
                LoadCurrentUser();
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                _viewModelNavigation.LoadPage(new ConnexionPage());
                _viewModelNavigation.NewContext();
            }
            else
            {
                Status = "Status: Fail to delete friend";
            }
            IsLoading = false;

        });



        public ProfileViewModel(RestClient restClient, ViewModelNavigation viewModelNavigation)
        {
            _restClient = restClient;
            _viewModelNavigation = viewModelNavigation;
            GameAddRequest = new GameAddRequest();
            LoadCurrentUser();
        }

        private async Task LoadCurrentUser()
        {
            var userName = ClaimsPrincipal.Current.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                Status = "Status: Failed to get user Informations";
                return;
            }

            Status = "Status: Getting user Informations ...";
            IsLoading = true;
            var request = new RestRequest($"User/{userName.Value}");
            var result = await _restClient.ExecuteTaskAsync<UserResponse>(request);
            if (result.IsSuccessful)
            {
                UserResponse = result.Data;
                OwnedGames = ConvertProfileGame(UserResponse.Games).ToList();
                Status = "Status: User informations loaded";

                var gamerequest = new RestRequest("Game/supportedGames");
                var gameresult = await _restClient.ExecuteTaskAsync<IEnumerable<BaseGameResponse>>(gamerequest);
                if (gameresult.IsSuccessful)
                {
                    Games = ConvertProfileGame(gameresult.Data).ToList();
                    SelectedGameInfo = Games.FirstOrDefault();
                }
                else
                {
                    Status = "Status: Failed to get supported Games";
                }
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                _viewModelNavigation.LoadPage(new ConnexionPage());
                _viewModelNavigation.NewContext();
            }
            else
            {
                Status = "Status: Failed to get user Informations";
            }
            IsLoading = false;
        }

        public static IEnumerable<GameInfo> ConvertProfileGame(IEnumerable<BaseGameResponse> gameResponses)
        {
            foreach (var gameResponse in gameResponses)
            {
                switch (gameResponse.ShortName)
                {
                    case "elite":
                        yield return ConvertGame<GameEliteInfo>(gameResponse);
                        break;
                    case "r6":
                        yield return ConvertGame<GameR6Info>(gameResponse);
                        break;
                    case "lol":
                        yield return ConvertGame<GameLolInfo>(gameResponse);
                        break;
                    case "fortnite":
                        yield return ConvertGame<GameFortniteInfo>(gameResponse);
                        break;
                    case "gw2":
                        yield return ConvertGame<GameGw2Info>(gameResponse);
                        break;
                    default:
                        yield return ConvertGame<GameLolInfo>(gameResponse);
                        break;
                }
            }
        }

        private static GameInfo ConvertGame<T>(BaseGameResponse baseGameResponse) where T : GameInfo, new()
        {
            var gameInfo = new T
            {
                NameGame = baseGameResponse.DisplayName,
                ShortNameGame = baseGameResponse.ShortName,
                ApiRequired = baseGameResponse.ApiKeyRequired
            };

            if (baseGameResponse is UserGameResponse userGameResponse)
            {
                gameInfo.Name = userGameResponse.Username;
            }

            return gameInfo;
        }
    }
}