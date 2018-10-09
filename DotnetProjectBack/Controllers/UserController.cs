using DotnetProjectBack.BusinessManagement;
using DotnetProjectBack.Models;
using DotnetProjectBack.Tools;
using Models.Requests;
using Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DotnetProjectBack.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        /// <summary>
        /// Get a token for an existing user
        /// </summary>
        /// <param name="user">The user informations you want to use</param>
        /// <returns>A JWT token</returns>
        [ProducesResponseType(200, Type = typeof(TokenResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<string> response = await _userBusiness.Login(user);
            if (!response.Success)
            {
                return BadRequest(new ErrorResponse(response.ErrorMessage));
            }
            return Ok(new TokenResponse() { Token = response.Result });
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user">The user informations you want to use</param>
        /// <returns>A JWT token for the newly created user</returns>
        [ProducesResponseType(201, Type = typeof(TokenResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<string> res = await _userBusiness.Register(user);
            if (res.Success)
            {
                return CreatedAtAction("GetProfile", new { username = user.Username }, new TokenResponse() { Token = res.Result });
            }
            else
            {
                return BadRequest(new ErrorResponse(res.ErrorMessage));
            }
        }

        /// <summary>
        /// Get informations about a specific user
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>The user profil</returns>
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [AllowAnonymous]
        [HttpGet("{username}", Name = "GetProfile")]
        public async Task<IActionResult> GetProfile([Required] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<UserResponse> res = await _userBusiness.GetProfile(username);
            return this.GetResultFromResponse(res);
        }

        /// <summary>
        /// Search for users
        /// </summary>
        /// <param name="username">The username to search. It will search usernames on the platform AND usernames in supported games
        /// for registered users</param>
        /// <returns>The list of user profiles</returns>
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserResponse>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [AllowAnonymous]
        [HttpGet("search", Name = "Search")]
        public async Task<IActionResult> Search([Required] [FromQuery] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<IEnumerable<UserResponse>> res = await _userBusiness.Search(username);
            return this.GetResultFromResponse(res);
        }

        /// <summary>
        /// Add another user as a friend
        /// </summary>
        /// <param name="username">The username of the friend you want to add</param>
        /// <returns>Success state</returns>
        [ProducesResponseType(200, Type = typeof(BooleanResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [Authorize]
        [HttpPost("friends")]
        public async Task<IActionResult> AddFriend([FromBody] UsernameRequest username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<BooleanResponse> res = await _userBusiness.AddFriend(HttpContext.GetCurrentUserId(), username.Username);
            return this.GetResultFromResponse(res);
        }

        /// <summary>
        /// Remove another user from friends list
        /// </summary>
        /// <param name="friendUsername">The username of the friend you want to delete</param>
        /// <returns>Success state</returns>
        [ProducesResponseType(200, Type = typeof(BooleanResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [Authorize]
        [HttpDelete("friends/{friendUsername}")]
        public async Task<IActionResult> DeleteFriend(string friendUsername)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<BooleanResponse> res = await _userBusiness.DeleteFriend(HttpContext.GetCurrentUserId(), friendUsername);
            return this.GetResultFromResponse(res);
        }
    }
}
