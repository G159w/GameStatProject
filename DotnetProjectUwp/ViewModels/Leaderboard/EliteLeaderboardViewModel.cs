using System.Collections.Generic;
using System.Linq;
using DotnetProjectUwp.Models;
using DotnetProjectUwp.Models.GamesInfo;
using DotNetProjectUwp.Views.Games;
using Models.Responses;
using RestSharp;

namespace DotnetProjectUwp.ViewModels.Leaderboard
{
    public class EliteLeaderboardViewModel : LeaderboardViewModel<EliteLeaderboardResponse>
    {

        public EliteLeaderboardViewModel(RestClient restClient, ViewModelNavigation viewModelNavigation) : base(restClient, viewModelNavigation)
        {
            SortOptions = new List<string> {"combat", "trade", "explore", "cqc"};
            SelectedOption = SortOptions.FirstOrDefault();
            LoadAsync(CreateRestRequest());
        }

        public override RestRequest CreateRestRequest()
        {
            var request = new RestRequest("Elite/leaderboard");
            request.AddQueryParameter("sortBy", SelectedOption);
            return request;
        }

        public override void TransformLeaderboardResponse()
        {
            UserRanks = null;
            var newUserRanks = new List<UserRank>();
            int i = 1;
            foreach (var leaderboardResponse in LeaderboardResponse)
            {
                newUserRanks.Add(new UserRank() {
                    Game = new GameEliteInfo
                    {
                        Name =  leaderboardResponse.GameUsername
                    },
                    MainAccount = leaderboardResponse.Username,
                    Rank = "#" + i
                });
                i++;
            }

            UserRanks = newUserRanks;
        }
    }
}
