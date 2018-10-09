using DotnetProjectBack.BusinessManagement.GamesStats;
using DotnetProjectBack.Models;
using DotnetProjectBack.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DotnetProjectBack.Controllers
{
    [Route("api/[controller]")]
    public class FortniteController : Controller
    {
        private readonly IFortniteBusiness _fortniteBusiness;

        public FortniteController(IFortniteBusiness fortniteBusiness)
        {
            _fortniteBusiness = fortniteBusiness;
        }

        /// <summary>
        /// Get stats for specified game player
        /// </summary>
        /// <param name="username">The Fortnite username of the user</param>
        /// <returns>The statistics for the user</returns>
        [ProducesResponseType(200, Type = typeof(FortniteStatsResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [AllowAnonymous]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetStats([Required] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<FortniteStatsResponse> res = await _fortniteBusiness.GetStats(username);
            return this.GetResultFromResponse(res);
        }

        /// <summary>
        /// Get leaderboards for Fortnite players
        /// </summary>
        /// <param name="sortBy">How to sort the leaderboard : "wins", "kills", "kd", "top1"</param>
        /// <returns>The leaderboard, already sorted</returns>
        [HttpGet("leaderboard")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FortniteLeaderboardResponse>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetLeaderboard([FromQuery] string sortBy = "wins")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<IEnumerable<FortniteLeaderboardResponse>> res = await _fortniteBusiness.GetLeaderboard(sortBy);
            return this.GetResultFromResponse(res);
        }
    }
}
