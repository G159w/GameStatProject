using DotnetProjectBack.Models;
using System.Threading.Tasks;
using Models.Responses;
using DotnetProjectBack.DatabaseModels;
using System.Collections.Generic;

namespace DotnetProjectBack.DataAccess.GamesStats
{
    public interface IGw2DataAccess
    {
        Task<GenericResponse<Gw2StatsResponse>> GetGw2StatsFromSite(string gameUsername, string personalApiKey);
        Task<GenericResponse<Gw2StatsResponse>> GetGw2StatsFromCache(string gameUsername);
        Task<GenericResponse<BooleanResponse>> UpdateCache(Gw2StatsResponse gw2Response, TUserGame userGame);
        Task<GenericResponse<IEnumerable<TGw2Stat>>> GetLeaderboard(string sortBy);
        Task<GenericResponse<BooleanResponse>> DeleteCache(long userId);
    }
}
