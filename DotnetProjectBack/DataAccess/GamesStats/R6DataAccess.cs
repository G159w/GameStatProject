using DotnetProjectBack.Models;
using System.Threading.Tasks;
using Models.Responses;
using DotnetProjectBack.DatabaseModels;
using System.Collections.Generic;
using DotnetProjectBack.DataAccess.ApiRequests;
using RestSharp;
using DotnetProjectBack.Tools;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using DotnetProjectBack.Models.ApiResponses;

namespace DotnetProjectBack.DataAccess.GamesStats
{
    public class R6DataAccess : IR6DataAccess
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IRequestFactory _requestFactory;
        private readonly RestClient _restClient;

        public R6DataAccess(DatabaseContext databaseContext, IRequestFactory requestFactory)
        {
            this._databaseContext = databaseContext;
            this._requestFactory = requestFactory;
            _restClient = new RestClient("https://api.r6stats.com/api/v1/");
        }

        public async Task<GenericResponse<BooleanResponse>> DeleteCache(long userId)
        {
            try
            {
                // Get
                TR6Stat r6Cache = _databaseContext.TR6Stat.FirstOrDefault(s => s.UserGame.UserId == userId);

                // Delete if found
                if (r6Cache != null)
                {
                    _databaseContext.TR6Stat.Remove(r6Cache);
                    await _databaseContext.SaveChangesAsync();
                }
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while deleting game cache", e);
            }
        }

        public async Task<GenericResponse<IEnumerable<TR6Stat>>> GetLeaderboard(string sortBy, string gameMode)
        {
            try
            {
                var leaderboardUsers = _databaseContext.TR6Stat
                    .Include(s => s.UserGame)
                        .ThenInclude(ug => ug.User)
                    .OrderByDescending(R6Tools.GetSortingFunctions(sortBy, gameMode))
                    .Take(50);

                return new GenericResponse<IEnumerable<TR6Stat>>(leaderboardUsers);
            }
            catch (Exception e)
            {
                return new GenericResponse<IEnumerable<TR6Stat>>("Cannot get leaderboard", e);
            }
        }

        public async Task<GenericResponse<R6StatsResponse>> GetStatsFromCache(string gameUsername)
        {
            TR6Stat dbStats = _databaseContext.TR6Stat.FirstOrDefault(s => s.UserGame.Username == gameUsername);

            if (dbStats == null)
            {
                return new GenericResponse<R6StatsResponse>($"Cannot get stats for {gameUsername}", null);
            }

            return new GenericResponse<R6StatsResponse>(dbStats.ConvertToR6StatsResponse());
        }

        public async Task<GenericResponse<R6StatsResponse>> GetStatsFromSite(string gameUsername)
        {
            var request = _requestFactory.GenerateRequest($"players/{gameUsername}", Method.GET, "r6");
            request.AddQueryParameter("platform", "uplay");

            var r6User = _restClient.Execute<R6TrackerResponse>(request);

            if (r6User.StatusCode.IsStatusOk())
            {
                return new GenericResponse<R6StatsResponse>(r6User.Data.ConvertToR6StatsResponse());
            }
            else
            {
                return new GenericResponse<R6StatsResponse>($"Cannot get stats for {gameUsername} (error {r6User.StatusCode})", null);
            }
        }

        public async Task<GenericResponse<BooleanResponse>> UpdateCache(R6StatsResponse r6StatsResponse, TUserGame userGame)
        {
            TR6Stat dbStats = _databaseContext.TR6Stat.FirstOrDefault(s => s.UserGame.Username == userGame.Username);

            bool newStats = false;
            if (dbStats == null)
            {
                newStats = true;
                dbStats = new TR6Stat { UserGameId = userGame.Id };
            }

            dbStats.CasualDeaths = r6StatsResponse.Player.Stats.Casual.Deaths;
            dbStats.CasualKd = r6StatsResponse.Player.Stats.Casual.Kd;
            dbStats.CasualKills = r6StatsResponse.Player.Stats.Casual.Kills;
            dbStats.CasualLosses = r6StatsResponse.Player.Stats.Casual.Losses;
            dbStats.CasualPlaytime = r6StatsResponse.Player.Stats.Casual.Playtime;
            dbStats.CasualWins = r6StatsResponse.Player.Stats.Casual.Wins;
            dbStats.CasualWlr = r6StatsResponse.Player.Stats.Casual.Wlr;
            dbStats.PlayerLevel = r6StatsResponse.Player.Stats.Progression.Level;
            dbStats.RankedDeaths = r6StatsResponse.Player.Stats.Ranked.Deaths;
            dbStats.RankedKd = r6StatsResponse.Player.Stats.Ranked.Kd;
            dbStats.RankedKills = r6StatsResponse.Player.Stats.Ranked.Kills;
            dbStats.RankedLosses = r6StatsResponse.Player.Stats.Ranked.Losses;
            dbStats.RankedPlaytime = r6StatsResponse.Player.Stats.Ranked.Playtime;
            dbStats.RankedWins = r6StatsResponse.Player.Stats.Ranked.Wins;
            dbStats.RankedWlr = r6StatsResponse.Player.Stats.Ranked.Wlr;

            try
            {
                if (newStats)
                {
                    _databaseContext.TR6Stat.Add(dbStats);
                }
                else
                {
                    _databaseContext.TR6Stat.Update(dbStats);
                }
                await _databaseContext.SaveChangesAsync();
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while saving in cache", e);
            }
        }
    }
}
