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
    public class R6Business : IR6Business
    {
        private readonly IR6DataAccess _r6DataAccess;
        private readonly IUserGameDataAccess _userGameDataAccess;

        public R6Business(IR6DataAccess r6DataAccess, IUserGameDataAccess userGameDataAccess)
        {
            _r6DataAccess = r6DataAccess;
            _userGameDataAccess = userGameDataAccess;
            _r6DataAccess = r6DataAccess;
            _userGameDataAccess = userGameDataAccess;
        }

        public async Task<GenericResponse<IEnumerable<R6LeaderboardResponse>>> GetLeaderboard(string sortBy, string gameMode)
        {
            GenericResponse<IEnumerable<TR6Stat>> leaderboards = await _r6DataAccess.GetLeaderboard(sortBy, gameMode);

            if (leaderboards.Success)
            {
                // Convert to correct model
                IEnumerable<R6LeaderboardResponse> convertedResult = leaderboards.Result.Select(i =>
                {
                    R6StatsResponse baseObject = i.ConvertToR6StatsResponse();
                    return new R6LeaderboardResponse
                    {
                        GameUsername = i.UserGame.Username,
                        Username = i.UserGame.User.Username,
                        UserId = i.UserGame.UserId,
                        Player = baseObject.Player
                    };
                });
                return new GenericResponse<IEnumerable<R6LeaderboardResponse>>(convertedResult);
            }
            else
            {
                return new GenericResponse<IEnumerable<R6LeaderboardResponse>>(leaderboards.ErrorMessage, leaderboards.Exception);
            }
        }

        public async Task<GenericResponse<R6StatsResponse>> GetStats(string gameUsername)
        {
            // Get corresponding player
            GenericResponse<TUserGame> r6Game = await _userGameDataAccess.GetUserGame("r6", gameUsername);
            if (!r6Game.Success)
            {
                return new GenericResponse<R6StatsResponse>($"{gameUsername} not found in the database", null);
            }

            // First try to get stats from EDSM
            GenericResponse<R6StatsResponse> r6Stats = await _r6DataAccess.GetStatsFromSite(gameUsername);
            if (r6Stats.Success)
            {
                await _r6DataAccess.UpdateCache(r6Stats.Result, r6Game.Result);

                return new GenericResponse<R6StatsResponse>(r6Stats.Result);
            }
            else
            {
                r6Stats = await _r6DataAccess.GetStatsFromCache(gameUsername);
            }

            // Response
            if (r6Stats.Success)
            {
                return new GenericResponse<R6StatsResponse>(r6Stats.Result);
            }
            else
            {
                return new GenericResponse<R6StatsResponse>(r6Stats.ErrorMessage, r6Stats.Exception);
            }
        }
    }
}
