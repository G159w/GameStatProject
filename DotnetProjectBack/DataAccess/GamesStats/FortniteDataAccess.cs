using DotnetProjectBack.DataAccess.ApiRequests;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using DotnetProjectBack.Tools;
using Microsoft.EntityFrameworkCore;
using Models.Responses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models.ApiResponses;

namespace DotnetProjectBack.DataAccess.GamesStats
{
    public class FortniteDataAccess : IFortniteDataAccess
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IRequestFactory _requestFactory;
        private readonly RestClient _restClient;

        public FortniteDataAccess(DatabaseContext databaseContext, IRequestFactory requestFactory)
        {
            _databaseContext = databaseContext;
            this._requestFactory = requestFactory;

            _restClient = new RestClient("https://api.fortnitetracker.com");
        }

        public async Task<GenericResponse<FortniteStatsResponse>> GetStatsFromSite(string gameUsername)
        {

            var request = _requestFactory.GenerateRequest($"v1/profile/pc/{gameUsername}", Method.GET, "fortnite");
            var fortniteUser = _restClient.Execute<FortniteResponse>(request);

            if (fortniteUser.StatusCode.IsStatusOk())
            {
                return new GenericResponse<FortniteStatsResponse>(fortniteUser.Data.ConvertFromApiResponse());
            }
            else
            {
                return new GenericResponse<FortniteStatsResponse>($"Cannot get stats for {gameUsername} (error {fortniteUser.StatusCode})", null);
            }
        }

        public async Task<GenericResponse<FortniteStatsResponse>> GetStatsFromCache(string gameUsername)
        {
            TFortnite dbStats = _databaseContext.TFortnite.FirstOrDefault(s => s.UserGame.Username == gameUsername);

            if (dbStats == null)
            {
                return new GenericResponse<FortniteStatsResponse>($"Cannot get stats for {gameUsername}", null);
            }

            return new GenericResponse<FortniteStatsResponse>(dbStats.ConvertToResponse());
        }

        public async Task<GenericResponse<BooleanResponse>> UpdateCache(FortniteStatsResponse fortniteResponse, TUserGame userGame)
        {
            TFortnite dbStats = _databaseContext.TFortnite.FirstOrDefault(s => s.UserGame.Username == userGame.Username);

            bool newStats = false;
            if (dbStats == null)
            {
                newStats = true;
                dbStats = new TFortnite { UserGameId = userGame.Id };
            }

            dbStats.DuoTop1 = fortniteResponse.DuoTop1;
            dbStats.DuoTop5 = fortniteResponse.DuoTop5;
            dbStats.DuoTop12 = fortniteResponse.DuoTop12;
            dbStats.SoloTop1 = fortniteResponse.SoloTop1;
            dbStats.SoloTop10 = fortniteResponse.SoloTop10;
            dbStats.SoloTop25 = fortniteResponse.SoloTop25;
            dbStats.SquadTop1 = fortniteResponse.SquadTop1;
            dbStats.SquadTop3 = fortniteResponse.SquadTop3;
            dbStats.SquadTop6 = fortniteResponse.SquadTop6;
            dbStats.Kills = fortniteResponse.Kills;
            dbStats.Kd = fortniteResponse.Kd;
            dbStats.Matches = fortniteResponse.Matches;
            dbStats.WinPercent = fortniteResponse.WinsPercent;
            dbStats.Wins = fortniteResponse.Wins;
            
            try
            {
                if (newStats)
                {
                    _databaseContext.TFortnite.Add(dbStats);
                }
                else
                {
                    _databaseContext.TFortnite.Update(dbStats);
                }
                await _databaseContext.SaveChangesAsync();
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while saving in cache", e);
            }
        }

        public async Task<GenericResponse<IEnumerable<TFortnite>>> GetLeaderboard(string sortBy)
        {
            try
            {
                // Get sorting functions
                var sortingFunctions = FortniteTools.GetSortingFunctions(sortBy);
                if (sortingFunctions.Item1 == null && sortingFunctions.Item2 == null)
                {
                    return new GenericResponse<IEnumerable<TFortnite>>($"Sort mode {sortBy} is not valid", null);
                }

                var leaderboardUsers = _databaseContext.TFortnite
                    .Include(s => s.UserGame)
                        .ThenInclude(ug => ug.User)
                    .OrderByDescending(sortingFunctions.Item1)
                    .ThenByDescending(sortingFunctions.Item2)
                    .Take(50);
                return new GenericResponse<IEnumerable<TFortnite>>(leaderboardUsers);
            }
            catch (Exception e)
            {
                return new GenericResponse<IEnumerable<TFortnite>>("Cannot get leaderboard", e);
            }
        }

        public async Task<GenericResponse<BooleanResponse>> DeleteCache(long userId)
        {
            try
            {
                // Get
                TFortnite cache = _databaseContext.TFortnite.Where(s => s.UserGame.UserId == userId).FirstOrDefault();

                // Delete if found
                if (cache != null)
                {
                    _databaseContext.TFortnite.Remove(cache);
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

