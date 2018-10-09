using DotnetProjectBack.DataAccess;
using DotnetProjectBack.DataAccess.GamesStats;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using Models.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetProjectBack.BusinessManagement.GamesStats
{
    class Gw2Business : IGw2Business
    {
        private readonly IGw2DataAccess _gw2DataAccess;
        private readonly IUserGameDataAccess _userGameDataAccess;

        public Gw2Business(IGw2DataAccess gw2DataAccess, IUserGameDataAccess userGameDataAccess)
        {
            _gw2DataAccess = gw2DataAccess;
            _userGameDataAccess = userGameDataAccess;
        }

        public async Task<GenericResponse<IEnumerable<Gw2LeaderboardResponse>>> GetLeaderboard(string sortBy)
        {
            GenericResponse<IEnumerable<TGw2Stat>> leaderboards = await _gw2DataAccess.GetLeaderboard(sortBy);

            if (leaderboards.Success)
            {
                // Convert to correct model
                IEnumerable<Gw2LeaderboardResponse> convertedResult = leaderboards.Result.Select(i =>
                {
                    return new Gw2LeaderboardResponse
                    {
                        GameUsername = i.UserGame.Username,
                        Username = i.UserGame.User.Username,
                        UserId = i.UserGame.UserId,
                        PvpRank = i.PvpRank,
                        PvpRankPoint = i.PvpRank,
                        PvpRankRollovers = i.PvpRankRollovers,
                        Aggregate = new Gw2StatsResponse.Gw2StatsResponseAggregate
                        {
                            Byes = i.Byes,
                            Desertions = i.Desertions,
                            Forfeits = i.Forfeits,
                            Losses = i.Losses,
                            Wins = i.Wins
                        }
                    };
                });
                return new GenericResponse<IEnumerable<Gw2LeaderboardResponse>>(convertedResult);
            }
            else
            {
                return new GenericResponse<IEnumerable<Gw2LeaderboardResponse>>(leaderboards.ErrorMessage, leaderboards.Exception);
            }
        }

        public async Task<GenericResponse<Gw2StatsResponse>> GetStats(string gameUsername)
        {
            // Get corresponding player
            GenericResponse<TUserGame> gw2Game = await _userGameDataAccess.GetUserGame("Gw2", gameUsername);
            if (!gw2Game.Success)
            {
                return new GenericResponse<Gw2StatsResponse>($"{gameUsername} not found in the database", null);
            }

            // First try to get stats from EDSM
            GenericResponse<Gw2StatsResponse> gw2Stats = await _gw2DataAccess.GetGw2StatsFromSite(gameUsername, gw2Game.Result.ApiKey);
            if (gw2Stats.Success)
            {
                await _gw2DataAccess.UpdateCache(gw2Stats.Result, gw2Game.Result);
                return new GenericResponse<Gw2StatsResponse>(gw2Stats.Result);
            }
            else
            {
                gw2Stats = await _gw2DataAccess.GetGw2StatsFromCache(gameUsername);
            }

            // Response
            if (gw2Stats.Success)
            {
                return new GenericResponse<Gw2StatsResponse>(gw2Stats.Result);
            }
            else
            {
                return new GenericResponse<Gw2StatsResponse>(gw2Stats.ErrorMessage, gw2Stats.Exception);
            }
        }
    }
}
