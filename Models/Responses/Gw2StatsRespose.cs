namespace Models.Responses
{
    public class Gw2StatsResponse
    {
        public int PvpRank { get; set; }
        public int PvpRankPoint { get; set; }
        public int PvpRankRollovers { get; set; }

        public Gw2StatsResponseAggregate Aggregate { get; set; }

        public class Gw2StatsResponseAggregate
        {
            public int Wins { get; set; }
            public int Losses { get; set; }
            public int Desertions { get; set; }
            public int Byes { get; set; }
            public int Forfeits { get; set; }
        }
    }
}
