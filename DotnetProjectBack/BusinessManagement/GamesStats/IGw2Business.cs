using DotnetProjectBack.Models;
using Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetProjectBack.BusinessManagement.GamesStats
{
    public interface IGw2Business
    {
        Task<GenericResponse<Gw2StatsResponse>> GetStats(string gameUsername);
        Task<GenericResponse<IEnumerable<Gw2LeaderboardResponse>>> GetLeaderboard(string sortBy);
    }
}
