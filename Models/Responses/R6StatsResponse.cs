using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Responses
{
    public class R6StatsResponse
    {
        public R6PlayerStats Player { get; set; }

        public class R6PlayerStats
        {
            public R6InnerStats Stats { get; set; }
        }

        public class R6InnerStats
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
            public String Playtime { get; set; }
        }

        public class R6ProgressionStats
        {
            public long Level { get; set; }
        }
    }
}
