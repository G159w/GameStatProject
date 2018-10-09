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
    public class FortniteLeaderboardViewModel : LeaderboardViewModel<FortniteLeaderboardResponse>
    {
        public FortniteLeaderboardViewModel(RestClient restClient, ViewModelNavigation viewModelNavigation) : base(restClient, viewModelNavigation)
        {
            SortOptions = new List<string> { "wins", "kills", "kd", "top1" };
            SelectedOption = SortOptions.FirstOrDefault();
            LoadAsync(CreateRestRequest());
        }
            
        public override RestRequest CreateRestRequest()
        {
            var request = new RestRequest("Fortnite/leaderboard");
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
                newUserRanks.Add(new UserRank()
                {
                    Game = new GameFortniteInfo()
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
    }
}

