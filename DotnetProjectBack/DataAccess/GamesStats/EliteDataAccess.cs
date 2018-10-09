using DotnetProjectBack.DataAccess.ApiRequests;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using DotnetProjectBack.Models.ApiResponses;
using DotnetProjectBack.Tools;
using Microsoft.EntityFrameworkCore;
using Models.Responses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetProjectBack.DatabaseModels;

namespace DotnetProjectBack.DataAccess.GamesStats
{
    class EliteDataAccess : IEliteDataAccess
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IRequestFactory _requestFactory;
        private readonly RestClient _restClient;

        public EliteDataAccess(DatabaseContext databaseContext, IRequestFactory requestFactory)
        {
            _databaseContext = databaseContext;
            _requestFactory = requestFactory;

            _restClient = new RestClient("https://www.edsm.net");
        }

        public async Task<GenericResponse<EdsmResponse>> GetEdsmStatsFromSite(string gameUsername)
        {
            var request = _requestFactory.GenerateRequest("api-commander-v1/get-ranks", Method.GET, "elite");
            request.AddQueryParameter("commanderName", gameUsername);
            var eliteUser = _restClient.Execute<EdsmResponse>(request);

            if (eliteUser.StatusCode.IsStatusOk() && eliteUser.Data.MsgNum == 100)
            {
                return new GenericResponse<EdsmResponse>(eliteUser.Data);
            }
            else
            {
                return new GenericResponse<EdsmResponse>($"Cannot get stats for {gameUsername} (error {eliteUser.StatusCode})", null);
            }
        }

        public async Task<GenericResponse<EdsmResponse>> GetEdsmStatsFromCache(string gameUsername)
        {
            TEliteStat dbStats = _databaseContext.TEliteStat.FirstOrDefault(s => s.UserGame.Username == gameUsername);

            if (dbStats == null)
            {
                return new GenericResponse<EdsmResponse>($"Cannot get stats for {gameUsername}", null);
            }

            return new GenericResponse<EdsmResponse>(dbStats.ConvertToEdsmResponse());
        }

        public async Task<GenericResponse<BooleanResponse>> UpdateCache(EdsmResponse edsmResponse, TUserGame userGame)
        {
            TEliteStat dbStats = _databaseContext.TEliteStat.FirstOrDefault(s => s.UserGame.Username == userGame.Username);

            bool newStats = false;
            if (dbStats == null)
            {
                newStats = true;
                dbStats = new TEliteStat { UserGameId = userGame.Id };
            }

            dbStats.CombatProgress = edsmResponse.Progress.Combat;
            dbStats.CombatRank = edsmResponse.Ranks.Combat;
            dbStats.CqcProgress = edsmResponse.Progress.CQC;
            dbStats.CqcRank = edsmResponse.Ranks.CQC;
            dbStats.EmpireProgress = edsmResponse.Progress.Empire;
            dbStats.EmpireRank = edsmResponse.Ranks.Empire;
            dbStats.ExplorerProgress = edsmResponse.Progress.Explore;
            dbStats.ExplorerRank = edsmResponse.Ranks.Explore;
            dbStats.FederationProgress = edsmResponse.Progress.Federation;
            dbStats.FederationRank = edsmResponse.Ranks.Federation;
            dbStats.TraderProgress = edsmResponse.Progress.Trade;
            dbStats.TraderRank = edsmResponse.Ranks.Trade;

            try
            {
                if (newStats)
                {
                    _databaseContext.TEliteStat.Add(dbStats);
                }
                else
                {
                    _databaseContext.TEliteStat.Update(dbStats);
                }
                await _databaseContext.SaveChangesAsync();
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while saving in cache", e);
            }
        }

        public async Task<GenericResponse<IEnumerable<TEliteStat>>> GetLeaderboard(string sortBy)
        {
            try
            {
                // Get sorting functions
                var sortingFunctions = EliteTools.GetSortingFunctions(sortBy);
                if (sortingFunctions.Item1 == null && sortingFunctions.Item2 == null)
                {
                    return new GenericResponse<IEnumerable<TEliteStat>>($"Sort mode {sortBy} is not valid", null);
                }

                var leaderboardUsers = _databaseContext.TEliteStat
                    .Include(s => s.UserGame)
                        .ThenInclude(ug => ug.User)
                    .OrderByDescending(sortingFunctions.Item1)
                    .ThenByDescending(sortingFunctions.Item2)
                    .Take(50);

                return new GenericResponse<IEnumerable<TEliteStat>>(leaderboardUsers);
            }
            catch (Exception e)
            {
                return new GenericResponse<IEnumerable<TEliteStat>>("Cannot get leaderboard", e);
            }
        }

        public async Task<GenericResponse<BooleanResponse>> DeleteCache(long userId)
        {
            try
            {
                // Get
                TEliteStat cache = _databaseContext.TEliteStat.Where(s => s.UserGame.UserId == userId).FirstOrDefault();

                // Delete if found
                if (cache != null)
                {
                    _databaseContext.TEliteStat.Remove(cache);
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
