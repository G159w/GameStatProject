using AutoMapper;
using DotnetProjectBack.DataAccess;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using DotnetProjectBack.Tools;
using Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetProjectBack.BusinessManagement
{
    public class GameBusiness : IGameBusiness
    {
        private readonly IGameDataAccess _gameDataAccess;
        private readonly IUserGameDataAccess _userGameDataAccess;
        private readonly CacheDeleteHelper _cacheDeleteHelper;
        private readonly IMapper _mapper;

        public GameBusiness(IGameDataAccess gameDataAccess, IUserGameDataAccess userGameDataAccess,
            CacheDeleteHelper cacheDeleteHelper, IMapper mapper)
        {
            _gameDataAccess = gameDataAccess;
            _userGameDataAccess = userGameDataAccess;
            _cacheDeleteHelper = cacheDeleteHelper;
            _mapper = mapper;
        }

        public async Task<GenericResponse<BooleanResponse>> AddUserGame(string shortName, string gameUsername, string gameApiKey,
            long userId)
        {
            // First check if the game exist
            TGame dbGame = await _gameDataAccess.GetDbGame(shortName);
            if (dbGame == null)
            {
                return new GenericResponse<BooleanResponse>($"Game {shortName} is not valid", null);
            }

            // Check if the game requires a per-user API key or not
            if (dbGame.ApiKeyRequired && string.IsNullOrWhiteSpace(gameApiKey))
            {
                return new GenericResponse<BooleanResponse>($"Game {shortName} requires an API key", null);
            }

            // If it's the case then add
            GenericResponse<BooleanResponse> res = await _userGameDataAccess.AddUserGame(dbGame, gameUsername, gameApiKey, userId);
            return res;
        }

        public async Task<GenericResponse<BooleanResponse>> RemoveUserGame(string shortName, string gameUsername, long userId)
        {
            // First check if the game exist
            TGame dbGame = await _gameDataAccess.GetDbGame(shortName);
            if (dbGame == null)
            {
                return new GenericResponse<BooleanResponse>($"Game {shortName} is not valid", null);
            }

            // Then delete the cache
            await _cacheDeleteHelper.DeleteUserCache(shortName, userId);

            // If it's the case then delete
            GenericResponse<BooleanResponse> res = await _userGameDataAccess.RemoveUserGame(dbGame, gameUsername, userId);
            return res;
        }

        public async Task<GenericResponse<IEnumerable<BaseGameResponse>>> GetSupportedGames()
        {
            IEnumerable<TGame> games = await _gameDataAccess.GetSupportedGames();
            return new GenericResponse<IEnumerable<BaseGameResponse>>(_mapper.Map<IEnumerable<TGame>, IEnumerable<BaseGameResponse>>(games));
        }
    }
}
