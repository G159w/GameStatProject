using Models.Responses;

namespace DotnetProjectBack.Models.ApiResponses
{
    public class R6TrackerResponse
    {
        public R6TrackerlayerStats Player { get; set; }

        public class R6TrackerlayerStats
        {
            public R6TrackerInnerStats Stats { get; set; }
        }

        public class R6TrackerInnerStats
        {
            public R6ModeStats Ranked { get; set; }
            public R6ModeStats Casual { get; set; }
            public R6ProgressionStats Progression { get; set; }
        }

        public class R6ModeStats
        {
            public long Wins { get; set; }
            public long Losses { get; set; }
            public double Wlr { get; set; }
            public long Kills { get; set; }
            public long Deaths { get; set; }
            public double Kd { get; set; }
            public long Playtime { get; set; }
        }

        public class R6ProgressionStats
        {
            public long Level { get; set; }
        }
    }
}
