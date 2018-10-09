using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models.ApiResponses;
using Models.Responses;
using System;
using static Models.Responses.Gw2StatsResponse;

namespace DotnetProjectBack.Tools
{
    public static class Gw2Tools
    {
        public static (Func<TGw2Stat, object>, Func<TGw2Stat, object>) GetSortingFunctions(string mode)
        {
            if (mode == "PvpRank")
            {
                object First(TGw2Stat s) => s.PvpRank;
                object Second(TGw2Stat s) => s.PvpRankPoints;
                return (First, Second);
            }
            else if (mode == "PvpRankPoint")
            {
                object First(TGw2Stat s) => s.PvpRankPoints;
                object Second(TGw2Stat s) => s.PvpRank;
                return (First, Second);
            }
            else if (mode == "Wins")
            {
                object First(TGw2Stat s) => s.Wins;
                object Second(TGw2Stat s) => s.PvpRank;
                return (First, Second);
            }
            return (null, null);
        }

        public static Gw2StatsResponse ConvertToGw2StatsResponse(this TGw2Stat dbStats)
        {
            return new Gw2StatsResponse
            {
                Aggregate = new Gw2StatsResponseAggregate
                {
                    Byes = dbStats.Byes,
                    Desertions = dbStats.Desertions,
                    Forfeits = dbStats.Forfeits,
                    Losses = dbStats.Losses,
                    Wins = dbStats.Wins
                },
                PvpRank = dbStats.PvpRank,
                PvpRankPoint = dbStats.PvpRankPoints,
                PvpRankRollovers = dbStats.PvpRankRollovers
            };
        }
    }
}
