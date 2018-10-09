using DotnetProjectBack.DataAccess;
using DotnetProjectBack.DataAccess.GamesStats;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using DotnetProjectBack.Models.ApiResponses;
using DotnetProjectBack.Tools;
using Models.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetProjectBack.BusinessManagement.GamesStats
{
    public class EliteBusiness : IEliteBusiness
    {
        private readonly IEliteDataAccess _eliteDataAccess;
        private readonly IUserGameDataAccess _userGameDataAccess;

        public EliteBusiness(IEliteDataAccess eliteDataAccess, IUserGameDataAccess userGameDataAccess)
        {
            _eliteDataAccess = eliteDataAccess;
            _userGameDataAccess = userGameDataAccess;
        }

        public async Task<GenericResponse<IEnumerable<EliteLeaderboardResponse>>> GetLeaderboard(string sortBy)
        {
            GenericResponse<IEnumerable<TEliteStat>> leaderboards = await _eliteDataAccess.GetLeaderboard(sortBy);

            if (leaderboards.Success)
            {
                // Convert to correct model
                IEnumerable<EliteLeaderboardResponse> convertedResult = leaderboards.Result.Select(i =>
                {
                    var baseObject = (EliteStatsResponse)i.ConvertToEdsmResponse();
                    return new EliteLeaderboardResponse
                    {
                        GameUsername = i.UserGame.Username,
                        Username = i.UserGame.User.Username,
                        UserId = i.UserGame.UserId,
                        Progress = baseObject.Progress,
                        Ranks = baseObject.Ranks,
                        RanksVerbose = baseObject.RanksVerbose
                    };
                });
                return new GenericResponse<IEnumerable<EliteLeaderboardResponse>>(convertedResult);
            }
            else
            {
                return new GenericResponse<IEnumerable<EliteLeaderboardResponse>>(leaderboards.ErrorMessage, leaderboards.Exception);
            }
        }

        public async Task<GenericResponse<EliteStatsResponse>> GetStats(string gameUsername)
        {
            // Get corresponding player
            GenericResponse<TUserGame> eliteGame = await _userGameDataAccess.GetUserGame("elite", gameUsername);
            if (!eliteGame.Success)
            {
                return new GenericResponse<EliteStatsResponse>($"{gameUsername} not found in the database", null);
            }

            // First try to get stats from EDSM
            GenericResponse<EdsmResponse> edsmStats = await _eliteDataAccess.GetEdsmStatsFromSite(gameUsername);
            if (edsmStats.Success)
            {
                await _eliteDataAccess.UpdateCache(edsmStats.Result, eliteGame.Result);

                return new GenericResponse<EliteStatsResponse>(edsmStats.Result.ConvertToResponse());
            }
            else
            {
                edsmStats = await _eliteDataAccess.GetEdsmStatsFromCache(gameUsername);
            }

            // Response
            if (edsmStats.Success)
            {
                return new GenericResponse<EliteStatsResponse>(edsmStats.Result.ConvertToResponse());
            }
            else
            {
                return new GenericResponse<EliteStatsResponse>(edsmStats.ErrorMessage, edsmStats.Exception);
            }
        }
    }
}
