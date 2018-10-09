using DotnetProjectBack.DataAccess.ApiRequests;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using DotnetProjectBack.Tools;
using Microsoft.EntityFrameworkCore;
using Models.Responses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetProjectBack.DataAccess.GamesStats
{
    class Gw2DataAccess : IGw2DataAccess
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IRequestFactory _requestFactory;
        private readonly RestClient _restClient;

        public Gw2DataAccess(DatabaseContext databaseContext, IRequestFactory requestFactory)
        {
            _databaseContext = databaseContext;
            _requestFactory = requestFactory;

            _restClient = new RestClient("https://api.guildwars2.com");
        }

        public async Task<GenericResponse<Gw2StatsResponse>> GetGw2StatsFromSite(string gameUsername, string personalApiKey)
        {
            var request = _requestFactory.GenerateRequest("/v2/pvp/stats", Method.GET, "gw2", personalApiKey);

            var gw2User = _restClient.Execute<Gw2StatsResponse>(request);

            if (gw2User.StatusCode.IsStatusOk())
            {
                return new GenericResponse<Gw2StatsResponse>(gw2User.Data);
            }
            else
            {
                return new GenericResponse<Gw2StatsResponse>($"Cannot get stats for {gameUsername} (error {gw2User.StatusCode})", null);
            }
        }

        public async Task<GenericResponse<Gw2StatsResponse>> GetGw2StatsFromCache(string gameUsername)
        {
            TGw2Stat dbStats = _databaseContext.TGw2Stat.FirstOrDefault(s => s.UserGame.Username == gameUsername);

            if (dbStats == null)
            {
                return new GenericResponse<Gw2StatsResponse>($"Cannot get stats for {gameUsername}", null);
            }

            return new GenericResponse<Gw2StatsResponse>(dbStats.ConvertToGw2StatsResponse());
        }

        public async Task<GenericResponse<BooleanResponse>> UpdateCache(Gw2StatsResponse gw2StatResponse, TUserGame userGame)
        {
            TGw2Stat dbStats = _databaseContext.TGw2Stat.FirstOrDefault(s => s.UserGame.Username == userGame.Username);

            bool newStats = false;
            if (dbStats == null)
            {
                newStats = true;
                dbStats = new TGw2Stat { UserGameId = userGame.Id };
            }

            dbStats.PvpRank = gw2StatResponse.PvpRank;
            dbStats.PvpRankPoints = gw2StatResponse.PvpRankPoint;
            dbStats.PvpRankRollovers = gw2StatResponse.PvpRankRollovers;
            dbStats.Wins = gw2StatResponse.Aggregate.Wins;
            dbStats.Losses = gw2StatResponse.Aggregate.Losses;
            dbStats.Desertions = gw2StatResponse.Aggregate.Desertions;
            dbStats.Byes = gw2StatResponse.Aggregate.Byes;
            dbStats.Forfeits = gw2StatResponse.Aggregate.Forfeits;
            dbStats.UserGameId = userGame.Id;

            try
            {
                if (newStats)
                {
                    _databaseContext.TGw2Stat.Add(dbStats);
                }
                else
                {
                    _databaseContext.TGw2Stat.Update(dbStats);
                }
                await _databaseContext.SaveChangesAsync();
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while saving in cache", e);
            }
        }

        public async Task<GenericResponse<IEnumerable<TGw2Stat>>> GetLeaderboard(string sortBy)
        {
            try
            {
                // Get sorting functions
                var sortingFunctions = Gw2Tools.GetSortingFunctions(sortBy);
                if (sortingFunctions.Item1 == null && sortingFunctions.Item2 == null)
                {
                    return new GenericResponse<IEnumerable<TGw2Stat>>($"Sort mode {sortBy} is not valid", null);
                }

                var leaderboardUsers = _databaseContext.TGw2Stat
                    .Include(s => s.UserGame)
                        .ThenInclude(ug => ug.User)
                    .OrderByDescending(sortingFunctions.Item1)
                    .ThenByDescending(sortingFunctions.Item2)
                    .Take(50);

                return new GenericResponse<IEnumerable<TGw2Stat>>(leaderboardUsers);
            }
            catch (Exception e)
            {
                return new GenericResponse<IEnumerable<TGw2Stat>>("Cannot get leaderboard", e);
            }
        }

        public async Task<GenericResponse<BooleanResponse>> DeleteCache(long userId)
        {
            try
            {
                // Get
                TGw2Stat cache = _databaseContext.TGw2Stat.Where(s => s.UserGame.UserId == userId).FirstOrDefault();

                // Delete if found
                if (cache != null)
                {
                    _databaseContext.TGw2Stat.Remove(cache);
                    await _databaseContext.SaveChangesAsync();
                }
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while deleting game cache", e);
            }
        }
    }
}
