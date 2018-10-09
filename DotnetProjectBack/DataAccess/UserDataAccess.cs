using System;
using System.Linq;
using System.Threading.Tasks;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models;
using Models.Requests;
using Models.Responses;
using DotnetProjectBack.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DotnetProjectBack.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;

        public UserDataAccess(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        public async Task<GenericResponse<string>> Login(LoginRequest loginRequest)
        {
            // Check if user exist
            TUser dbUser;
            try
            {
                dbUser = _databaseContext.TUser.FirstOrDefault(u => u.Username == loginRequest.Username);
            }
            catch (Exception e)
            {
                return GetLoginErrorResponse(e);
            }
            if (dbUser == null)
            {
                return GetLoginErrorResponse();
            }

            // Check hash     
            string hashed = AuthTools.GetHash(loginRequest.Password, Convert.FromBase64String(dbUser.PasswordSalt));

            if (hashed == dbUser.PasswordHash)
            {
                return new GenericResponse<string>(AuthTools.GetJwtToken(_configuration, dbUser));
            }
            return GetLoginErrorResponse();
        }

        public async Task<GenericResponse<string>> Register(RegisterRequest registerRequest)
        {
            // Create user
            byte[] salt = AuthTools.GetRandomSalt();
            string hash = AuthTools.GetHash(registerRequest.Password, salt);
            TUser newUser = new TUser()
            {
                RegisterDate = DateTime.UtcNow,
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                PasswordHash = hash,
                PasswordSalt = Convert.ToBase64String(salt)
            };

            try
            {
                await _databaseContext.TUser.AddAsync(newUser);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<string>("Cannot register with these informations", e);
            }

            return new GenericResponse<string>(AuthTools.GetJwtToken(_configuration, newUser));
        }

        public async Task<GenericResponse<TUser>> GetProfile(string username)
        {
            // Check if user exist
            TUser dbUser;
            try
            {
                dbUser = _databaseContext.TUser.Where(u => u.Username == username)
                    .Include(u => u.TFriendUser)
                    .ThenInclude(f => f.Friend)
                    .ThenInclude(f => f.TUserGame)
                    .Include(u => u.TUserGame)
                    .ThenInclude(ug => ug.Game)
                    .FirstOrDefault();

                if (dbUser == null)
                {
                    return new GenericResponse<TUser>($"User {username} not found", null);
                }
            }
            catch (Exception e)
            {
                return new GenericResponse<TUser>($"User {username} not found", e);
            }

            return new GenericResponse<TUser>(dbUser);
        }

        public async Task<GenericResponse<BooleanResponse>> AddFriend(long userId, string friendUsername)
        {
            try
            {
                TUser dbFriend = _databaseContext.TUser.FirstOrDefault(u => u.Username == friendUsername);
                if (dbFriend == null)
                {
                    return new GenericResponse<BooleanResponse>($"Username {friendUsername} not found", null);
                }

                TFriend friend = new TFriend
                {
                    UserId = userId,
                    FriendId = dbFriend.Id
                };
                _databaseContext.TFriend.Add(friend);
                await _databaseContext.SaveChangesAsync();

                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>($"Error while adding friend", e);
            }
        }

        public async Task<GenericResponse<BooleanResponse>> DeleteFriend(long userId, string friendUsername)
        {
            try
            {
                TFriend dbFriend = _databaseContext.TFriend.FirstOrDefault(f => f.UserId == userId && f.Friend.Username == friendUsername);

                // Error case if not found
                if (dbFriend == null)
                {
                    return new GenericResponse<BooleanResponse>($"{friendUsername} is not a friend", null);
                }

                _databaseContext.TFriend.Remove(dbFriend);
                await _databaseContext.SaveChangesAsync();
                return new GenericResponse<BooleanResponse>(new BooleanResponse { Success = true });
            }
            catch (DbUpdateException e)
            {
                return new GenericResponse<BooleanResponse>("Error while deleting friend", e);
            }
        }

        private static GenericResponse<string> GetLoginErrorResponse(Exception e = null)
        {
            return new GenericResponse<string>("User not found or password is not correct", e);
        }

        public async Task<GenericResponse<IEnumerable<TUser>>> Search(string username)
        {
            string usernameForQuery = username.ToLowerInvariant();
            try
            {
                var dbUsers = _databaseContext.TUser
                    .Where(u => u.Username.ToLowerInvariant().Contains(usernameForQuery) ||
                        u.TUserGame.FirstOrDefault(ug => ug.Username.ToLowerInvariant().Contains(usernameForQuery)) != null)
                    .Include(u => u.TFriendUser)
                    .ThenInclude(f => f.Friend)
                    .ThenInclude(f => f.TUserGame)
                    .Include(u => u.TUserGame)
                    .ThenInclude(ug => ug.Game);

                if (dbUsers == null || dbUsers.Count() == 0)
                {
                    return new GenericResponse<IEnumerable<TUser>>(new List<TUser>());
                }

                return new GenericResponse<IEnumerable<TUser>>(dbUsers);
            }
            catch (Exception e)
            {
                return new GenericResponse<IEnumerable<TUser>>("Error while performing search", e);
            }
        }
    }
}
