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
    public class LolController : Controller
    {
        private readonly ILolBusiness _lolBusiness;

        public LolController(ILolBusiness lolBusiness)
        {
            _lolBusiness = lolBusiness;
        }

        /// <summary>
        /// Get stats for specified game player
        /// </summary>
        /// <param name="username">The Lol username of the user</param>
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

            GenericResponse<LolStatsResponse> res = await _lolBusiness.GetStats(username);
            return this.GetResultFromResponse(res);
        }

        /// <summary>
        /// Get leaderboards for Lol players
        /// </summary>
        /// <param name="sortBy">How to sort the leaderboard : "SoloWins", "FlexWins"</param>
        /// <returns>The leaderboard, already sorted</returns>
        [HttpGet("leaderboard")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LolLeaderboardResponse>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetLeaderboard([FromQuery] string sortBy = "SoloWins")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<IEnumerable<LolLeaderboardResponse>> res = await _lolBusiness.GetLeaderboard(sortBy);
            return this.GetResultFromResponse(res);
        }
    }
}
