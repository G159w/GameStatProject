using System;
using System.Collections.Generic;

namespace Models.Responses
{
    public class FriendResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public DateTime RegisterDate { get; set; }
        public IEnumerable<UserGameResponse> Games { get; set; }

    }
}
