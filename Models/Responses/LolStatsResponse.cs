using System.Collections.Generic;

namespace Models.Responses
{
    public class LolStatsResponse
    {
           public LolListStatsResponse SoloQ { get; set; }
           public LolListStatsResponse FlexQ { get; set; }
    }
}
