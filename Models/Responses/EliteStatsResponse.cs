namespace Models.Responses
{
    public class EliteStatsResponse
    {
        public EliteStatsResponseRanks Ranks { get; set; }
        public EliteStatsResponseRanks Progress { get; set; }
        public EliteStatsResponseRanksVerbose RanksVerbose { get; set; }


        public class EliteStatsResponseRanks
        {
            public int Combat { get; set; }
            public int Trade { get; set; }
            public int Explore { get; set; }
            public int CQC { get; set; }
            public int Federation { get; set; }
            public int Empire { get; set; }
        }

        public class EliteStatsResponseRanksVerbose
        {
            public string Combat { get; set; }
            public string Trade { get; set; }
            public string Explore { get; set; }
            public string CQC { get; set; }
            public string Federation { get; set; }
            public string Empire { get; set; }
        }
    }
}
