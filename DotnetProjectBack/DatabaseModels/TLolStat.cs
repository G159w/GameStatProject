using System;
using System.Collections.Generic;

namespace DotnetProjectBack.DatabaseModels
{
    public partial class TLolStat
    {
        public long Id { get; set; }
        public long UserGameId { get; set; }
        public string SoloTier { get; set; }
        public string SoloRank { get; set; }
        public string SoloNameLeague { get; set; }
        public int SoloWins { get; set; }
        public int SoloLosses { get; set; }
        public int SoloLp { get; set; }
        public string FlexTier { get; set; }
        public string FlexRank { get; set; }
        public string FlexNameLeague { get; set; }
        public int FlexWins { get; set; }
        public int FlexLosses { get; set; }
        public int FlexLp { get; set; }

        public TUserGame UserGame { get; set; }
    }
}
