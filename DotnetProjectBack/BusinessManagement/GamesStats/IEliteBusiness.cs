using DotnetProjectBack.Models;
using Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetProjectBack.BusinessManagement.GamesStats
{
    public interface IEliteBusiness
    {
        Task<GenericResponse<EliteStatsResponse>> GetStats(string gameUsername);
        Task<GenericResponse<IEnumerable<EliteLeaderboardResponse>>> GetLeaderboard(string sortBy);
    }
}
