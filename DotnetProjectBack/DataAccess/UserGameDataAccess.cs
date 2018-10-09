using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetProjectBack.DataAccess
{
    public class UserGameDataAccess : IUserGameDataAccess
    {
        private readonly DatabaseContext _databaseContext;

        public UserGameDataAccess(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<GenericResponse<BooleanResponse>> AddUserGame(TGame dbGame, string username, string apiKey, long userId)
        {
            try
            {
                await _databaseContext.TUserGame.AddAsync(new TUserGame
                {
                    GameId = dbGame.Id,
                    Username = username,
                    UserId = userId,
                    ApiKey = apiKey
                });

                await _databaseContext.SaveChangesAsync();
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while adding game", e);
            }
        }

        public async Task<GenericResponse<BooleanResponse>> RemoveUserGame(TGame dbGame, string gameUsername, long userId)
        {
            try
            {
                TUserGame dbUserGame = _databaseContext.TUserGame
                    .FirstOrDefault(ug => ug.UserId == userId && ug.GameId == dbGame.Id && ug.Username == gameUsername);

                // Error case if not found
                if (dbUserGame == null)
                {
                    return new GenericResponse<BooleanResponse>($"Cannot find {gameUsername} for game {dbGame.Id}", null);
                }

                _databaseContext.TUserGame.Remove(dbUserGame);
                await _databaseContext.SaveChangesAsync();
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while removing game", e);
            }
        }

        public async Task<GenericResponse<TUserGame>> GetUserGame(string gameShortName, string username)
        {
            var userGame = _databaseContext.TUserGame.FirstOrDefault(g => g.Game.ShortName == gameShortName && g.Username == username);
            if (userGame == null)
            {
                return new GenericResponse<TUserGame>($"{username} not found for {gameShortName}", null);
            }
            else
            {
                return new GenericResponse<TUserGame>(userGame);
            }
        }
    }
}
