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
    public class EliteController : Controller
    {
        private readonly IEliteBusiness _eliteBusiness;

        public EliteController(IEliteBusiness eliteBusiness)
        {
            this._eliteBusiness = eliteBusiness;
        }

        /// <summary>
        /// Get stats for specified game player
        /// </summary>
        /// <param name="username">The Elite:Dangerous username of the user</param>
        /// <returns>The statistics for the user</returns>
        [ProducesResponseType(200, Type = typeof(EliteStatsResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [AllowAnonymous]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetStats([Required] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<EliteStatsResponse> res = await _eliteBusiness.GetStats(username);
            return this.GetResultFromResponse(res);
        }

        /// <summary>
        /// Get leaderboards for Elite : Dangerous players
        /// </summary>
        /// <param name="sortBy">How to sort the leaderboard : "combat", "trade", "explore", "cqc"</param>
        /// <returns>The leaderboard, already sorted</returns>
        [HttpGet("leaderboard")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EliteLeaderboardResponse>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetLeaderboard([FromQuery] string sortBy = "combat")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<IEnumerable<EliteLeaderboardResponse>> res = await _eliteBusiness.GetLeaderboard(sortBy);
            return this.GetResultFromResponse(res);
        }
    }
}