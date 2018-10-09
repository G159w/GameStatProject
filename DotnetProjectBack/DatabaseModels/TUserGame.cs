using System;
using System.Collections.Generic;

namespace DotnetProjectBack.DatabaseModels
{
    public partial class TUserGame
    {
        public TUserGame()
        {
            TEliteStat = new HashSet<TEliteStat>();
            TFortnite = new HashSet<TFortnite>();
            TGw2Stat = new HashSet<TGw2Stat>();
            TLolStat = new HashSet<TLolStat>();
            TR6Stat = new HashSet<TR6Stat>();
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public long GameId { get; set; }
        public string Username { get; set; }
        public string ApiKey { get; set; }

        public TGame Game { get; set; }
        public TUser User { get; set; }
        public ICollection<TEliteStat> TEliteStat { get; set; }
        public ICollection<TFortnite> TFortnite { get; set; }
        public ICollection<TGw2Stat> TGw2Stat { get; set; }
        public ICollection<TLolStat> TLolStat { get; set; }
        public ICollection<TR6Stat> TR6Stat { get; set; }
    }
}
