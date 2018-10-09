using DotnetProjectBack.DataAccess;
using DotnetProjectBack.DataAccess.GamesStats;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using DotnetProjectBack.Tools;
using Models.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DotnetProjectBack.BusinessManagement.GamesStats
{
    public class LolBusiness : ILolBusiness
    {
        private readonly ILolDataAccess _lolDataAccess;
        private readonly IUserGameDataAccess _userGameDataAccess;

        public LolBusiness(ILolDataAccess lolDataAccess, IUserGameDataAccess userGameDataAccess)
        {
            _lolDataAccess = lolDataAccess;
            _userGameDataAccess = userGameDataAccess;
        }

        public async Task<GenericResponse<IEnumerable<LolLeaderboardResponse>>> GetLeaderboard(string sortBy)
        {
            GenericResponse<IEnumerable<TLolStat>> leaderboards = await _lolDataAccess.GetLeaderboard(sortBy);

            if (leaderboards.Success)
            {
                // Convert to correct model
                IEnumerable<LolLeaderboardResponse> convertedResult = leaderboards.Result.Select(i =>
                {
                    LolStatsResponse baseObject = i.ConvertToResponse();
                    return new LolLeaderboardResponse
                    {
                        GameUsername = i.UserGame.Username,
                        Username = i.UserGame.User.Username,
                        UserId = i.UserGame.UserId,
                        FlexQ = baseObject.FlexQ,
                        SoloQ = baseObject.SoloQ
                    };
                });
                return new GenericResponse<IEnumerable<LolLeaderboardResponse>>(convertedResult);
            }
            else
            {
                return new GenericResponse<IEnumerable<LolLeaderboardResponse>>(leaderboards.ErrorMessage, leaderboards.Exception);
            }
        }


        public async Task<GenericResponse<LolStatsResponse>> GetStats(string gameUsername)
        {
            // Get corresponding player
            GenericResponse<TUserGame> lolGame = await _userGameDataAccess.GetUserGame("lol", gameUsername);
            if (!lolGame.Success)
            {
                return new GenericResponse<LolStatsResponse>($"{gameUsername} not found in the database", null);
            }

            // First try to get stats from Site
            GenericResponse<LolStatsResponse> lolStats = await _lolDataAccess.GetStatsFromSite(gameUsername);
            if (lolStats.Success)
            {
                await _lolDataAccess.UpdateCache(lolStats.Result, lolGame.Result);

                return new GenericResponse<LolStatsResponse>(lolStats.Result);
            }
            else
            {
                lolStats = await _lolDataAccess.GetStatsFromCache(gameUsername);
            }

            // Response
            if (lolStats.Success)
            {
                return new GenericResponse<LolStatsResponse>(lolStats.Result);
            }
            else
            {
                return new GenericResponse<LolStatsResponse>(lolStats.ErrorMessage, lolStats.Exception);
            }
        }
    }
}
