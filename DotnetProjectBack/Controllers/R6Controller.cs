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
    public class R6Controller : Controller
    {
        private readonly IR6Business _r6Business;

        public R6Controller(IR6Business r6Business)
        {
            _r6Business = r6Business;
        }

        /// <summary>
        /// Get stats for specified game player
        /// </summary>
        /// <param name="username">The R6 username of the user</param>
        /// <returns>The statistics for the user</returns>
        [ProducesResponseType(200, Type = typeof(R6StatsResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [AllowAnonymous]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetStats([Required] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<R6StatsResponse> res = await _r6Business.GetStats(username);
            return this.GetResultFromResponse(res);
        }

        /// <summary>
        /// Get leaderboards for Elite : Dangerous players
        /// </summary>
        /// <param name="sortBy">How to sort the leaderboard : "wins", "losses", "wlr", "kills", "deaths", "kd", "playtime"</param>
        /// <param name="gameMode">Game mode for the leaderboard : "ranked" or "casual"</param>
        /// <returns>The leaderboard, already sorted</returns>
        [HttpGet("leaderboard")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<R6LeaderboardResponse>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetLeaderboard([FromQuery] string sortBy = "wins", [FromQuery] string gameMode = "ranked")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<IEnumerable<R6LeaderboardResponse>> res = await _r6Business.GetLeaderboard(sortBy, gameMode);
            return this.GetResultFromResponse(res);
        }
    }
}