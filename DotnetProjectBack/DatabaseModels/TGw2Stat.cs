using System;
using System.Collections.Generic;

namespace DotnetProjectBack.DatabaseModels
{
    public partial class TGw2Stat
    {
        public long Id { get; set; }
        public int PvpRank { get; set; }
        public int PvpRankPoints { get; set; }
        public int PvpRankRollovers { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Desertions { get; set; }
        public int Byes { get; set; }
        public int Forfeits { get; set; }
        public long UserGameId { get; set; }

        public TUserGame UserGame { get; set; }
    }
}
