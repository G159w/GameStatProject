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
    public class FortniteBusiness : IFortniteBusiness
    {
        private readonly IFortniteDataAccess _fortniteDataAccess;
        private readonly IUserGameDataAccess _userGameDataAccess;

        public FortniteBusiness(IFortniteDataAccess fortniteDataAccess, IUserGameDataAccess userGameDataAccess)
        {
            _fortniteDataAccess = fortniteDataAccess;
            _userGameDataAccess = userGameDataAccess;
        }

        public async Task<GenericResponse<IEnumerable<FortniteLeaderboardResponse>>> GetLeaderboard(string sortBy)
        {       
            GenericResponse<IEnumerable<TFortnite>> leaderboards = await _fortniteDataAccess.GetLeaderboard(sortBy);

            if (leaderboards.Success)
            {
                // Convert to correct model
                IEnumerable<FortniteLeaderboardResponse> convertedResult = leaderboards.Result.Select(i => new FortniteLeaderboardResponse
                {
                    GameUsername = i.UserGame.Username,
                    Username = i.UserGame.User.Username,
                    UserId = i.UserGame.UserId,
                    SoloTop1 = i.SoloTop1,
                    SoloTop10 = i.SoloTop10,
                    SoloTop25 = i.SoloTop25,
                    DuoTop1 = i.DuoTop1,
                    DuoTop5 = i.DuoTop5,
                    DuoTop12 = i.DuoTop12,
                    SquadTop1 = i.SquadTop1,
                    SquadTop3 = i.SquadTop3,
                    SquadTop6 = i.SquadTop6,
                    Kd = i.Kd,
                    Kills = i.Kills,
                    Matches = i.Matches,
                    Wins = i.Wins,
                    WinsPercent = i.WinPercent
                });
                return new GenericResponse<IEnumerable<FortniteLeaderboardResponse>>(convertedResult);
            }
            else
            {
                return new GenericResponse<IEnumerable<FortniteLeaderboardResponse>>(leaderboards.ErrorMessage, leaderboards.Exception);
            }
        }


        public async Task<GenericResponse<FortniteStatsResponse>> GetStats(string gameUsername)
        {
            // Get corresponding player
            GenericResponse<TUserGame> fortniteGame = await _userGameDataAccess.GetUserGame("fortnite", gameUsername);
            if (!fortniteGame.Success)
            {
                return new GenericResponse<FortniteStatsResponse>($"{gameUsername} not found in the database", null);
            }

            // First try to get stats from Site
            GenericResponse<FortniteStatsResponse> fortniteStats = await _fortniteDataAccess.GetStatsFromSite(gameUsername);
            if (fortniteStats.Success)
            {
                await _fortniteDataAccess.UpdateCache(fortniteStats.Result, fortniteGame.Result);

                return new GenericResponse<FortniteStatsResponse>(fortniteStats.Result);
            }
            else
            {
                fortniteStats = await _fortniteDataAccess.GetStatsFromCache(gameUsername);
            }

            // Response
            if (fortniteStats.Success)
            {
                return new GenericResponse<FortniteStatsResponse>(fortniteStats.Result);
            }
            else
            {
                return new GenericResponse<FortniteStatsResponse>(fortniteStats.ErrorMessage, fortniteStats.Exception);
            }
        }
    }
}
