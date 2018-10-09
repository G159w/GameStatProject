using DotnetProjectBack.Models;
using Models.Requests;
using System.Threading.Tasks;
using Models.Responses;
using System.Collections.Generic;

namespace DotnetProjectBack.BusinessManagement
{
    public interface IUserBusiness
    {
        Task<GenericResponse<string>> Login(LoginRequest loginRequest);
        Task<GenericResponse<string>> Register(RegisterRequest registerRequest);
        Task<GenericResponse<UserResponse>> GetProfile(string username);
        Task<GenericResponse<BooleanResponse>> AddFriend(long userId, string friendUsername);
        Task<GenericResponse<BooleanResponse>> DeleteFriend(long userId, string friendUsername);
        Task<GenericResponse<IEnumerable<UserResponse>>> Search(string username);
    }
}
