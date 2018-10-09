using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using Models.Responses;
using System.Threading.Tasks;

namespace DotnetProjectBack.DataAccess
{
    public interface IUserGameDataAccess
    {
        Task<GenericResponse<TUserGame>> GetUserGame(string gameShortName, string username);
        Task<GenericResponse<BooleanResponse>> AddUserGame(TGame dbGame, string username, string apiKey, long userId);
        Task<GenericResponse<BooleanResponse>> RemoveUserGame(TGame dbGame, string gameUsername, long userId);
    }
}
