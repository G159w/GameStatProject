using DotnetProjectBack.Models;
using System.Threading.Tasks;
using Models.Responses;
using DotnetProjectBack.DatabaseModels;
using System.Collections.Generic;

namespace DotnetProjectBack.DataAccess.GamesStats
{
    public interface IR6DataAccess
    {
        Task<GenericResponse<R6StatsResponse>> GetStatsFromSite(string gameUsername);
        Task<GenericResponse<R6StatsResponse>> GetStatsFromCache(string gameUsername);
        Task<GenericResponse<BooleanResponse>> UpdateCache(R6StatsResponse r6StatsResponse, TUserGame userGame);
        Task<GenericResponse<BooleanResponse>> DeleteCache(long userId);
        Task<GenericResponse<IEnumerable<TR6Stat>>> GetLeaderboard(string sortBy, string gameMode);
    }
}
