namespace Models.Responses
{
    public class Gw2LeaderboardResponse :Gw2StatsResponse
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string GameUsername { get; set; }
    }
}
