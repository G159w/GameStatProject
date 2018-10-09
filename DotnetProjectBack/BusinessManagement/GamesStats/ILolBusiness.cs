using DotnetProjectBack.Models;
using Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetProjectBack.BusinessManagement.GamesStats
{
    public interface ILolBusiness
    {
        Task<GenericResponse<LolStatsResponse>> GetStats(string gameUsername);
        Task<GenericResponse<IEnumerable<LolLeaderboardResponse>>> GetLeaderboard(string sortBy);
    }
}
