using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotnetProjectBack.DataAccess;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using Models.Requests;
using Models.Responses;

namespace DotnetProjectBack.BusinessManagement
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserDataAccess _userDataAccess;
        private readonly IMapper _autoMapper;

        public UserBusiness(IUserDataAccess userDataAccess, IMapper autoMapper)
        {
            this._userDataAccess = userDataAccess;
            this._autoMapper = autoMapper;
        }

        public Task<GenericResponse<BooleanResponse>> AddFriend(long userId, string friendUsername)
        {
            return _userDataAccess.AddFriend(userId, friendUsername);
        }

        public Task<GenericResponse<BooleanResponse>> DeleteFriend(long userId, string friendUsername)
        {
            return _userDataAccess.DeleteFriend(userId, friendUsername);
        }

        public async Task<GenericResponse<UserResponse>> GetProfile(string username)
        {
            GenericResponse<TUser> dbUser = await _userDataAccess.GetProfile(username);

            // Convert database model to response model to avoid sending salt and hash :)
            if (!dbUser.Success)
            {
                return new GenericResponse<UserResponse>(dbUser.ErrorMessage, dbUser.Exception);
            }
            else
            {
                return new GenericResponse<UserResponse>(_autoMapper.Map<UserResponse>(dbUser.Result));
            }
        }

        public async Task<GenericResponse<string>> Login(LoginRequest loginRequest)
        {
            return await _userDataAccess.Login(loginRequest);
        }

        public async Task<GenericResponse<string>> Register(RegisterRequest registerRequest)
        {
            return await _userDataAccess.Register(registerRequest);
        }

        public async Task<GenericResponse<IEnumerable<UserResponse>>> Search(string username)
        {
            // Check query params first
            if (string.IsNullOrWhiteSpace(username))
            {
                return new GenericResponse<IEnumerable<UserResponse>>("username query parameter is required", null);
            }

            GenericResponse<IEnumerable<TUser>> dbUsers = await _userDataAccess.Search(username);
            return new GenericResponse<IEnumerable<UserResponse>>(_autoMapper.Map<IEnumerable<UserResponse>>(dbUsers.Result));
          
        }
    }
}
