namespace Models.Responses
{
    public class LolListStatsResponse
    {
        public string Tier { get; set; }
        public string Rank { get; set; }
        public string LeagueName { get; set; }
        public int LeaguePoints { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
