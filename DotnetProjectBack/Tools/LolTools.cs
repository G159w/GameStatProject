using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models.ApiResponses;
using Models.Responses;
using System;

namespace DotnetProjectBack.Tools
{
    public static class LolTools
    {
        public static (Func<TLolStat, object>, Func<TLolStat, object>) GetSortingFunctions(string mode)
        {
            if (mode == "SoloWins")
            {
                object First(TLolStat s) => s.SoloWins;
                object Second(TLolStat s) => s.SoloWins;
                return (First, Second);
            }
            else if (mode == "FlexWins")
            {
                object First(TLolStat s) => s.FlexWins;
                object Second(TLolStat s) => s.FlexWins;
                return (First, Second);
            }
            return (null, null);
        }


        public static LolStatsResponse ConvertToResponse(this TLolStat lolDbStats)
        {
            if (lolDbStats == null)
            {
                return null;
            }
            LolStatsResponse lolResponse = new LolStatsResponse
            {   
                    SoloQ = new LolListStatsResponse
                    {
                        LeagueName = lolDbStats.SoloNameLeague,
                        Wins = lolDbStats.SoloWins,
                        Losses = lolDbStats.SoloLosses,
                        LeaguePoints = lolDbStats.SoloLp,
                        Tier = lolDbStats.SoloTier,
                        Rank = lolDbStats.SoloRank
                    },
                    FlexQ = new LolListStatsResponse
                    {
                        LeagueName = lolDbStats.FlexNameLeague,
                        Wins = lolDbStats.FlexWins,
                        Losses = lolDbStats.FlexLosses,
                        LeaguePoints = lolDbStats.FlexLp,
                        Tier = lolDbStats.FlexTier,
                        Rank = lolDbStats.FlexRank
                    }
            };
            return lolResponse;
        }

        public static LolStatsResponse NoRankResponse()
        {
            LolStatsResponse lolResponse = new LolStatsResponse
            {
                SoloQ = new LolListStatsResponse
                {
                    LeagueName = "Wukong's knight",
                    Wins = 1337,
                    Losses = 42,
                    LeaguePoints = 100,
                    Tier = "I",
                    Rank = "Challenger"
                },
                FlexQ = new LolListStatsResponse
                {
                    LeagueName = "Wukong's knight",
                    Wins = 1337,
                    Losses = 42,
                    LeaguePoints = 100,
                    Tier = "I",
                    Rank = "Challenger"
                }
            };
            return lolResponse;
        }
    }
}
