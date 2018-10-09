using DotnetProjectBack.Models;
using Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetProjectBack.BusinessManagement.GamesStats
{
    public interface IR6Business
    {
        Task<GenericResponse<R6StatsResponse>> GetStats(string gameUsername);
        Task<GenericResponse<IEnumerable<R6LeaderboardResponse>>> GetLeaderboard(string sortBy, string gameMode);
    }
}
