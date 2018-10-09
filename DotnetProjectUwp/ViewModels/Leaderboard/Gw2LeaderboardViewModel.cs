using System.Collections.Generic;
using System.Linq;
using DotnetProjectUwp.Models;
using DotnetProjectUwp.Models.GamesInfo;
using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Leaderboard
{
    public class Gw2LeaderboardViewModel : LeaderboardViewModel<Gw2LeaderboardResponse>
    {

        public Gw2LeaderboardViewModel(RestClient restClient, ViewModelNavigation viewModelNavigation) : base(restClient, viewModelNavigation)
        {
            SortOptions = new List<string> { "PvpRank", "PvpRankPoint", "PvpRankRollovers" };
            SelectedOption = SortOptions.FirstOrDefault();
            LoadAsync(CreateRestRequest());
        }

        public override RestRequest CreateRestRequest()
        {
            var request = new RestRequest("Gw2/leaderboard");
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
                newUserRanks.Add(new UserRank
                {
                    Game = new GameGw2Info()
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

