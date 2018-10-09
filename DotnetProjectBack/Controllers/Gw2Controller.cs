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
    public class Gw2Controller : Controller
    {
        private readonly IGw2Business _gw2Business;

        public Gw2Controller(IGw2Business gw2Business)
        {
            this._gw2Business = gw2Business;
        }

        /// <summary>
        /// Get stats for specified game player
        /// </summary>
        /// <param name="username">The Gw2 username of the user</param>
        /// <returns>The statistics for the user</returns>
        [ProducesResponseType(200, Type = typeof(Gw2StatsResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [AllowAnonymous]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetStats([Required] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<Gw2StatsResponse> res = await _gw2Business.GetStats(username);
            return this.GetResultFromResponse(res);
        }

        /// <summary>
        /// Get leaderboards for Gw2 players
        /// </summary>
        /// <param name="sortBy">How to sort the leaderboard : "PvpRank", "PvpRankPoint", "PvpRankRollovers"</param>
        /// <returns>The leaderboard, already sorted</returns>
        [HttpGet("leaderboard")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Gw2LeaderboardResponse>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetLeaderboard([FromQuery] string sortBy = "PvpRank")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<IEnumerable<Gw2LeaderboardResponse>> res = await _gw2Business.GetLeaderboard(sortBy);
            return this.GetResultFromResponse(res);
        }
    }
}