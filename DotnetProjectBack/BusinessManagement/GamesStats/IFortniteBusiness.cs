using DotnetProjectBack.Models;
using Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetProjectBack.BusinessManagement.GamesStats
{
    public interface IFortniteBusiness
    {
        Task<GenericResponse<FortniteStatsResponse>> GetStats(string gameUsername);
        Task<GenericResponse<IEnumerable<FortniteLeaderboardResponse>>> GetLeaderboard(string sortBy);
    }
}
