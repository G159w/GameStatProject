namespace DotnetProjectUwp.Models.GamesInfo
{
    public abstract class GameInfo 
    {
        public string Name {get; set;}

        public string ShortNameGame { get; set; }

        public string NameGame { get; set; }

        public string Source {get; protected set; }

        public bool ApiRequired { get; set; }
    }
}