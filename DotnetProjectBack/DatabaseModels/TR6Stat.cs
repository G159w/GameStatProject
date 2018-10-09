using System;
using System.Collections.Generic;

namespace DotnetProjectBack.DatabaseModels
{
    public partial class TR6Stat
    {
        public long Id { get; set; }
        public long UserGameId { get; set; }
        public long PlayerLevel { get; set; }
        public long RankedWins { get; set; }
        public long RankedLosses { get; set; }
        public double RankedWlr { get; set; }
        public long RankedKills { get; set; }
        public long RankedDeaths { get; set; }
        public double RankedKd { get; set; }
        public string RankedPlaytime { get; set; }
        public long CasualWins { get; set; }
        public long CasualLosses { get; set; }
        public double CasualWlr { get; set; }
        public long CasualKills { get; set; }
        public long CasualDeaths { get; set; }
        public double CasualKd { get; set; }
        public string CasualPlaytime { get; set; }

        public TUserGame UserGame { get; set; }
    }
}
