using DotnetProjectBack.BusinessManagement;
using DotnetProjectBack.Models;
using DotnetProjectBack.Tools;
using Models.Requests;
using Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;

namespace DotnetProjectBack.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameBusiness gameBusiness;

        public GameController(IGameBusiness gameBusiness)
        {
            this.gameBusiness = gameBusiness;
        }

        /// <summary>
        /// Add a game for the currently connected user
        /// </summary>
        /// <param name="game">The short name of the game</param>
        /// <param name="gameAddRequest">Your username and API key (optional, only for games requiring per-user API key) in this game</param>
        /// <returns>Success state</returns>
        [ProducesResponseType(200, Type = typeof(BooleanResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [Authorize]
        [HttpPost("{game}")]
        public async Task<IActionResult> AddUserGame([Required] string game, [FromBody] GameAddRequest gameAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<BooleanResponse> response = await gameBusiness.AddUserGame(game, gameAddRequest.Username, gameAddRequest.ApiKey,
                HttpContext.GetCurrentUserId());
            return this.GetResultFromResponse(response);
        }

        /// <summary>
        /// Remove a game for the currently connected user
        /// </summary>
        /// <param name="game">The short name of the game</param>
        /// <param name="gameUsername">The username in this game</param>
        /// <returns>Success state</returns>
        [ProducesResponseType(200, Type = typeof(BooleanResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [Authorize]
        [HttpDelete("{game}/{gameUsername}")]
        public async Task<IActionResult> RemoveUserGame([Required] string game, [Required] string gameUsername)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<BooleanResponse> response = await gameBusiness.RemoveUserGame(game, gameUsername, HttpContext.GetCurrentUserId());
            return this.GetResultFromResponse(response);
        }

        /// <summary>
        /// Get the list of the supported games
        /// </summary>
        /// <returns>The list of supported games</returns>
        [ProducesResponseType(200, Type = typeof(IEnumerable<BaseGameResponse>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [AllowAnonymous]
        [HttpGet("supportedGames")]
        public async Task<IActionResult> GetSupportedGamesList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<IEnumerable<BaseGameResponse>> response = await gameBusiness.GetSupportedGames();
            return this.GetResultFromResponse(response);
        }
    }
}
