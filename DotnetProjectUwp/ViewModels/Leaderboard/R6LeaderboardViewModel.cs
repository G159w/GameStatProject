using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetProjectUwp.Models;
using DotnetProjectUwp.Models.GamesInfo;
using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Leaderboard
{
    public class R6LeaderboardViewModel : LeaderboardViewModel<R6LeaderboardResponse>
    {
        private string _selectedMode;
        public string SeletedMode
        {
            get => _selectedMode;
            set
            {
                _selectedMode = value;
                RaisePropertyChanged();
            }
        }

        private IEnumerable<string> _modes;

        public IEnumerable<string> Modes
        {
            get => _modes;
            set
            {
                _modes = value;
                RaisePropertyChanged();
            }
        }

        public R6LeaderboardViewModel(RestClient restClient, ViewModelNavigation viewModelNavigation) : base(restClient, viewModelNavigation)
        {
            SortOptions = new List<string> { "wins", "losses", "wlr", "kills", "deaths", "kd", "playtime" };
            SelectedOption = SortOptions.FirstOrDefault();
            Modes = new List<string> {"ranked", "casual"};
            SeletedMode = Modes.FirstOrDefault(); 
            LoadAsync(CreateRestRequest());
        }

        public override void TransformLeaderboardResponse()
        {
            UserRanks = null;
            var newUserRanks = new List<UserRank>();
            int i = 1;
            foreach (var leaderboardResponse in LeaderboardResponse)
            {
                newUserRanks.Add(new UserRank()
                {
                    Game = new GameR6Info()
                    {
                        Name = leaderboardResponse.GameUsername
                    },
                    MainAccount = leaderboardResponse.Username,
                    Rank = "#" + i
                });
                i++;
            }

            UserRanks = newUserRanks;
        }

        public override RestRequest CreateRestRequest()
        {
            var request = new RestRequest("r6/leaderboard");
            request.AddQueryParameter("gameMode", SeletedMode);
            request.AddQueryParameter("sortBy", SelectedOption);
            return request;
        }
    }
}
