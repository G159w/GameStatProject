using System;
using System.Collections.Generic;

namespace DotnetProjectBack.DatabaseModels
{
    public partial class TFriend
    {
        public long UserId { get; set; }
        public long FriendId { get; set; }

        public TUser Friend { get; set; }
        public TUser User { get; set; }
    }
}
