using DotnetProjectBack.Models;
using System.Threading.Tasks;
using Models.Responses;
using DotnetProjectBack.DatabaseModels;
using System.Collections.Generic;

namespace DotnetProjectBack.DataAccess.GamesStats
{
    public interface IFortniteDataAccess
    {
        Task<GenericResponse<FortniteStatsResponse>> GetStatsFromSite(string gameUsername);
        Task<GenericResponse<FortniteStatsResponse>> GetStatsFromCache(string gameUsername);
        Task<GenericResponse<BooleanResponse>> UpdateCache(FortniteStatsResponse fortniteStatsResponse, TUserGame userGame);
        Task<GenericResponse<IEnumerable<TFortnite>>> GetLeaderboard(string sortBy);
        Task<GenericResponse<BooleanResponse>> DeleteCache(long userId);
    }
}
