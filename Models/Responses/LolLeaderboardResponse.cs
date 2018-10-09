namespace Models.Responses
{
    public class LolLeaderboardResponse : LolStatsResponse
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string GameUsername { get; set; }
    }
}
