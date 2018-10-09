using DotnetProjectBack.Models;
using System.Threading.Tasks;
using Models.Responses;
using DotnetProjectBack.DatabaseModels;
using System.Collections.Generic;

namespace DotnetProjectBack.DataAccess.GamesStats
{
    public interface ILolDataAccess
    {
        Task<GenericResponse<LolStatsResponse>> GetStatsFromSite(string gameUsername);
        Task<GenericResponse<LolStatsResponse>> GetStatsFromCache(string gameUsername);
        Task<GenericResponse<BooleanResponse>> UpdateCache(LolStatsResponse lolStatsResponse, TUserGame userGame);
        Task<GenericResponse<IEnumerable<TLolStat>>> GetLeaderboard(string sortBy);
        Task<GenericResponse<BooleanResponse>> DeleteCache(long userId);
    }
}
