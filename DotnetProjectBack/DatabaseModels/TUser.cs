using System;
using System.Collections.Generic;

namespace DotnetProjectBack.DatabaseModels
{
    public partial class TUser
    {
        public TUser()
        {
            TFriendFriend = new HashSet<TFriend>();
            TFriendUser = new HashSet<TFriend>();
            TUserGame = new HashSet<TUserGame>();
        }

        public long Id { get; set; }
        public string Email { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Username { get; set; }

        public ICollection<TFriend> TFriendFriend { get; set; }
        public ICollection<TFriend> TFriendUser { get; set; }
        public ICollection<TUserGame> TUserGame { get; set; }
    }
}
