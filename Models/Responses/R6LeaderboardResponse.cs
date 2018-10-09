namespace Models.Responses
{
    public class R6LeaderboardResponse : R6StatsResponse
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string GameUsername { get; set; }
    }
}
