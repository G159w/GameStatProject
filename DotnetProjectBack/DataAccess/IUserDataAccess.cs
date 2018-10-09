using DotnetProjectBack.Models;
using Models.Requests;
using System.Threading.Tasks;
using Models.Responses;
using System.Collections.Generic;
using DotnetProjectBack.DatabaseModels;

namespace DotnetProjectBack.DataAccess
{
    public interface IUserDataAccess
    {
        Task<GenericResponse<string>> Login(LoginRequest loginRequest);
        Task<GenericResponse<string>> Register(RegisterRequest registerRequest);
        Task<GenericResponse<TUser>> GetProfile(string username);
        Task<GenericResponse<BooleanResponse>> AddFriend(long userId, string friendUsername);
        Task<GenericResponse<BooleanResponse>> DeleteFriend(long userId, string friendUsername);
        Task<GenericResponse<IEnumerable<TUser>>> Search(string username);
    }
}
