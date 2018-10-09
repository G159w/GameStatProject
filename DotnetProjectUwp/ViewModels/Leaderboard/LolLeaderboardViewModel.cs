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
    public class LolLeaderboardViewModel : LeaderboardViewModel<LolLeaderboardResponse>
    {

        public LolLeaderboardViewModel(RestClient restClient, ViewModelNavigation viewModelNavigation) : base(restClient, viewModelNavigation)
        {
            SortOptions = new List<string> { "SoloWins", "FlexWins" };
            SelectedOption = SortOptions.FirstOrDefault();
            LoadAsync(CreateRestRequest());
        }

        public override RestRequest CreateRestRequest()
        {
            var request = new RestRequest("Lol/leaderboard");
            request.AddQueryParameter("sortBy", SelectedOption);
            return request;
        }

        public override void TransformLeaderboardResponse()
        {
            UserRanks = null;
            var newUserRanks = new List<UserRank>();
            int i = 1;
            foreach (var lolLeaderboardResponse in LeaderboardResponse)
            {
                newUserRanks.Add(new UserRank()
                {
                    Game = new GameLolInfo()
                    {
                        Name = lolLeaderboardResponse.GameUsername
                    },
                    MainAccount = lolLeaderboardResponse.Username,
                    Rank = "#" + i
                });
                i++;
            }

            UserRanks = newUserRanks;
        }
    }
}

