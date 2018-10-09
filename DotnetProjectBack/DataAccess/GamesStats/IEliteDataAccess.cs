using DotnetProjectBack.Models;
using System.Threading.Tasks;
using DotnetProjectBack.Models.ApiResponses;
using Models.Responses;
using DotnetProjectBack.DatabaseModels;
using System.Collections.Generic;

namespace DotnetProjectBack.DataAccess.GamesStats
{
    public interface IEliteDataAccess
    {
        Task<GenericResponse<EdsmResponse>> GetEdsmStatsFromSite(string gameUsername);
        Task<GenericResponse<EdsmResponse>> GetEdsmStatsFromCache(string gameUsername);
        Task<GenericResponse<BooleanResponse>> UpdateCache(EdsmResponse edsmResponse, TUserGame userGame);
        Task<GenericResponse<IEnumerable<TEliteStat>>> GetLeaderboard(string sortBy);
        Task<GenericResponse<BooleanResponse>> DeleteCache(long userId);
    }
}
