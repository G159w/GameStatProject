using System;
using System.Collections.Generic;

namespace DotnetProjectBack.DatabaseModels
{
    public partial class TEliteStat
    {
        public long UserGameId { get; set; }
        public int CombatRank { get; set; }
        public int TraderRank { get; set; }
        public int ExplorerRank { get; set; }
        public int CqcRank { get; set; }
        public int FederationRank { get; set; }
        public int EmpireRank { get; set; }
        public int CombatProgress { get; set; }
        public int TraderProgress { get; set; }
        public int ExplorerProgress { get; set; }
        public int CqcProgress { get; set; }
        public int FederationProgress { get; set; }
        public int EmpireProgress { get; set; }
        public long Id { get; set; }

        public TUserGame UserGame { get; set; }
    }
}
