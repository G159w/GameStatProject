using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetProjectBack.Models;
using Models.Responses;

namespace DotnetProjectBack.BusinessManagement
{
    public interface IGameBusiness
    {
        Task<GenericResponse<BooleanResponse>> AddUserGame(string shortName, string gameUsername, string gameApiKey, long userId);
        Task<GenericResponse<BooleanResponse>> RemoveUserGame(string shortName, string gameUsername, long userId);
        Task<GenericResponse<IEnumerable<BaseGameResponse>>> GetSupportedGames();
    }
}
