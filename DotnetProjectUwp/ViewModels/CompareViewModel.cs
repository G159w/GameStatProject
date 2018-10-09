using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DotnetProjectUwp.Models.GamesInfo;
using DotnetProjectUwp.ViewModels.Profile;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels
{
    public class CompareViewModel : ViewModelBase
    {
        private readonly RestClient _restClient;
        private string _user1;

        public string User1
        {
            get => _user1;
            set
            {
                _user1 = value;
                RaisePropertyChanged();
            }
        }

        private string _user2;

        public string User2
        {
            get => _user2;
            set
            {
                _user2 = value;
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

        private IDictionary<string, object> _userStat1;

        public IDictionary<string, object> UserStat1
        {
            get => _userStat1;
            set
            {
                _userStat1 = value;
                RaisePropertyChanged();
            }
        }

        private IDictionary<string, object> _userStat2;

        public IDictionary<string, object> UserStat2
        {
            get => _userStat2;
            set
            {
                _userStat2 = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CompareCommand => new RelayCommand(async () => await Compare());

        public CompareViewModel(RestClient restClient)
        {
            _restClient = restClient;
            LoadGames();
        }

        public async Task LoadGames()
        {
            var gamerequest = new RestRequest("Game/supportedGames");
            IsLoading = true;
            Status = "Status: Loading games ...";
            var gameresult = await _restClient.ExecuteTaskAsync<IEnumerable<BaseGameResponse>>(gamerequest);
            if (gameresult.IsSuccessful)
            {
                Status = "Status: Loaded";
                Games = ProfileViewModel.ConvertProfileGame(gameresult.Data).ToList();
                SelectedGameInfo = Games.FirstOrDefault();
            }
            else
            {
                Status = "Status: failed to get games";
            }

            IsLoading = false;
        }

        public async Task Compare()
        {
            var user1Request = new RestRequest($"{SelectedGameInfo.ShortNameGame}/{User1}");
            Status = $"Status: Loading {User1} ...";
            IsLoading = true;
            var user1Result = await _restClient.ExecuteTaskAsync<object>(user1Request);
            if (user1Result.IsSuccessful)
            {
                var user2Request = new RestRequest($"{SelectedGameInfo.ShortNameGame}/{User2}");
                Status = $"Status: Loading {User2} ...";
                var user2Result = await _restClient.ExecuteTaskAsync<object>(user2Request);
                if (user2Result.IsSuccessful)
                {
                    Status = "Status: Loaded";
                    var propertyUser1 = (IDictionary<string, object>) user1Result.Data;
                    var propertyUser2 = (IDictionary<string, object>) user2Result.Data;
                    UserStat1 = DictionnaryTransform(propertyUser1);
                    UserStat2 = DictionnaryTransform(propertyUser2);
                }
                else
                {
                    Status = $"Status: Failed to get {User2} stats";
                }
            }
            else
            {
                Status = $"Status: Failed to get {User1} stats";
            }
            IsLoading = false;
        }

        private IDictionary<string, object> DictionnaryTransform(IDictionary<string, object> dictionary)
        {
            var newDict = new Dictionary<string, object>();
            foreach (var obj in dictionary)
            {
                if (obj.Value is Dictionary<string, object> objdict)
                {
                    var tmpdict = DictionnaryTransform(objdict);
                    newDict = newDict.Concat(tmpdict.ToDictionary(x => FirstCharToUpper(obj.Key) + ' ' + FirstCharToUpper(x.Key), x => x.Value))
                        .ToDictionary(x => x.Key, x => x.Value);
                }
                else
                {
                    newDict.Add(obj.Key, obj.Value);
                }
            }

            return newDict;
        }

        private string FirstCharToUpper(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
