namespace Models.Responses
{
    public class FortniteLeaderboardResponse : FortniteStatsResponse
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string GameUsername { get; set; }
    }
}