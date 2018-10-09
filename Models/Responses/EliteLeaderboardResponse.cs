namespace Models.Responses
{
    public class EliteLeaderboardResponse : EliteStatsResponse
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string GameUsername { get; set; }
    }
}
