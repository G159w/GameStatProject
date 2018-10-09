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

namespace DotnetProjectBack.DataAccess.GamesStats
{
    public class LolDataAccess : ILolDataAccess
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IRequestFactory _requestFactory;
        private readonly RestClient _restClient;

        public LolDataAccess(DatabaseContext databaseContext, IRequestFactory requestFactory)
        {
            _databaseContext = databaseContext;
            _requestFactory = requestFactory;

            _restClient = new RestClient("https://euw1.api.riotgames.com");
        }

        public async Task<GenericResponse<LolStatsResponse>> GetStatsFromSite(string gameUsername)
        {
            var requestId = _requestFactory.GenerateRequest($"lol/summoner/v3/summoners/by-name/{gameUsername}", Method.GET, "lol");
            var lolId = _restClient.Execute<LolResponse>(requestId);

            if (lolId.StatusCode.IsStatusOk())
            {
                var id = new GenericResponse<LolResponse>(lolId.Data).Result.Id;
                var request = _requestFactory.GenerateRequest($"lol/league/v3/positions/by-summoner/{id}", Method.GET, "lol");
                var lolUser = _restClient.Execute<List<LolListStatsResponse>>(request);
              
                if (lolUser.StatusCode.IsStatusOk())
                {
                    if (lolUser.Data.Count() == 2)
                    {
                        var lolStats = new LolStatsResponse
                        {
                            SoloQ = lolUser.Data[1],
                            FlexQ = lolUser.Data[0]
                        };
                        return new GenericResponse<LolStatsResponse>(lolStats);
                    }
                    else if (lolUser.Data.Count() < 2)
                    {
                        return new GenericResponse<LolStatsResponse>(LolTools.NoRankResponse());
                    }
                    else
                    {
                        var lolStats = new LolStatsResponse
                        {
                            SoloQ = lolUser.Data[1],
                            FlexQ = lolUser.Data[2]
                        };
                        return new GenericResponse<LolStatsResponse>(lolStats);
                    }
                }
                else
                {
                    return new GenericResponse<LolStatsResponse>($"Cannot get stats for {gameUsername} (error {lolUser.StatusCode})", null);
                }

            }
            else
            {
                return new GenericResponse<LolStatsResponse>($"Cannot get stats for {gameUsername} (error {lolId.StatusCode})", null);
            }
        }

        public async Task<GenericResponse<LolStatsResponse>> GetStatsFromCache(string gameUsername)
        {
            TLolStat dbStats = _databaseContext.TLolStat.FirstOrDefault(s => s.UserGame.Username == gameUsername);

            if (dbStats == null)
            {
                return new GenericResponse<LolStatsResponse>($"Cannot get stats for {gameUsername}", null);
            }

            return new GenericResponse<LolStatsResponse>(dbStats.ConvertToResponse());
        }

        public async Task<GenericResponse<BooleanResponse>> UpdateCache(LolStatsResponse lolResponse, TUserGame userGame)
        {
            TLolStat dbStats = _databaseContext.TLolStat.FirstOrDefault(s => s.UserGame.Username == userGame.Username);

            bool newStats = false;
            if (dbStats == null)
            {
                newStats = true;
                dbStats = new TLolStat { UserGameId = userGame.Id };
            }

            
            dbStats.FlexTier = lolResponse.FlexQ.Tier;
            dbStats.FlexLosses = lolResponse.FlexQ.Losses;
            dbStats.FlexLp = lolResponse.FlexQ.LeaguePoints;
            dbStats.FlexNameLeague = lolResponse.FlexQ.LeagueName;
            dbStats.FlexWins = lolResponse.FlexQ.Wins;
            dbStats.FlexRank = lolResponse.FlexQ.Rank;
            
            dbStats.SoloTier = lolResponse.SoloQ.Tier;
            dbStats.SoloLosses = lolResponse.SoloQ.Losses;
            dbStats.SoloLp = lolResponse.SoloQ.LeaguePoints;
            dbStats.SoloNameLeague = lolResponse.SoloQ.LeagueName;
            dbStats.SoloWins = lolResponse.SoloQ.Wins;
            dbStats.SoloRank = lolResponse.SoloQ.Rank;

            try
            {
                if (newStats)
                {
                    _databaseContext.TLolStat.Add(dbStats);
                }
                else
                {
                    _databaseContext.TLolStat.Update(dbStats);
                }
                await _databaseContext.SaveChangesAsync();
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while saving in cache", e);
            }
        }

        public async Task<GenericResponse<IEnumerable<TLolStat>>> GetLeaderboard(string sortBy)
        {
            try
            {
                // Get sorting functions
                var sortingFunctions = LolTools.GetSortingFunctions(sortBy);
                if (sortingFunctions.Item1 == null && sortingFunctions.Item2 == null)
                {
                    return new GenericResponse<IEnumerable<TLolStat>>($"Sort mode {sortBy} is not valid", null);
                }

                var leaderboardUsers = _databaseContext.TLolStat
                    .Include(s => s.UserGame)
                        .ThenInclude(ug => ug.User)
                    .OrderByDescending(sortingFunctions.Item1)
                    .ThenByDescending(sortingFunctions.Item2)
                    .Take(50);
                return new GenericResponse<IEnumerable<TLolStat>>(leaderboardUsers);
            }
            catch (Exception e)
            {
                return new GenericResponse<IEnumerable<TLolStat>>("Cannot get leaderboard", e);
            }
        }

        public async Task<GenericResponse<BooleanResponse>> DeleteCache(long userId)
        {
            try
            {
                // Get
                TLolStat cache = _databaseContext.TLolStat.Where(s => s.UserGame.UserId == userId).FirstOrDefault();

                // Delete if found
                if (cache != null)
                {
                    _databaseContext.TLolStat.Remove(cache);
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
