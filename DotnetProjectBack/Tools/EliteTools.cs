using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Models.ApiResponses;
using Models.Responses;
using System;
using static Models.Responses.EliteStatsResponse;

namespace DotnetProjectBack.Tools
{
    public static class EliteTools
    {
        public static (Func<TEliteStat, object>, Func<TEliteStat, object>) GetSortingFunctions(string mode)
        {
            if (mode == "combat")
            {
                object First(TEliteStat s) => s.CombatRank;
                object Second(TEliteStat s) => s.CombatProgress;
                return (First, Second);
            }
            else if (mode == "trade")
            {
                object First(TEliteStat s) => s.TraderRank;
                object Second(TEliteStat s) => s.TraderProgress;
                return (First, Second);
            }
            else if (mode == "explore")
            {
                object First(TEliteStat s) => s.ExplorerRank;
                object Second(TEliteStat s) => s.ExplorerProgress;
                return (First, Second);
            }
            else if (mode == "cqc")
            {
                object First(TEliteStat s) => s.CqcRank;
                object Second(TEliteStat s) => s.CqcProgress;
                return (First, Second);
            }
            return (null, null);
        }

        public static EliteStatsResponse ConvertToResponse(this EdsmResponse edsmResponse)
        {
            EliteStatsResponse res = new EliteStatsResponse
            {
                Progress = edsmResponse.Progress,
                Ranks = edsmResponse.Ranks,
                RanksVerbose = edsmResponse.RanksVerbose
            };
            return res;
        }

        public static EdsmResponse ConvertToEdsmResponse(this TEliteStat eliteDbStats)
        {
            if (eliteDbStats == null)
            {
                return null;
            }

            EdsmResponse edsmResponse = new EdsmResponse
            {
                MsgNum = 100,
                Progress = new EliteStatsResponseRanks
                {
                    Combat = eliteDbStats.CombatProgress,
                    CQC = eliteDbStats.CqcProgress,
                    Empire = eliteDbStats.EmpireProgress,
                    Explore = eliteDbStats.ExplorerProgress,
                    Federation = eliteDbStats.FederationProgress,
                    Trade = eliteDbStats.TraderProgress
                },
                Ranks = new EliteStatsResponseRanks
                {
                    Combat = eliteDbStats.CombatRank,
                    CQC = eliteDbStats.CqcRank,
                    Empire = eliteDbStats.EmpireRank,
                    Explore = eliteDbStats.ExplorerRank,
                    Federation = eliteDbStats.FederationRank,
                    Trade = eliteDbStats.TraderRank
                },
                RanksVerbose = new EliteStatsResponseRanksVerbose
                {
                    Combat = GetCombatRankVerbose(eliteDbStats.CombatRank),
                    CQC = GetCqcRankVerbose(eliteDbStats.CqcRank),
                    Empire = GetEmpireRankVerbose(eliteDbStats.EmpireRank),
                    Explore = GetExplorationRankVerbose(eliteDbStats.ExplorerRank),
                    Federation = GetFederationRankVerbose(eliteDbStats.FederationRank),
                    Trade = GetTradeRankVerbose(eliteDbStats.TraderRank)
                }
            };

            return edsmResponse;
        }

        public static string GetCombatRankVerbose(int combatRank)
        {
            switch (combatRank)
            {
                case 0:
                    return "Harmless";
                case 1:
                    return "Mostly Harmless";
                case 2:
                    return "Novice";
                case 3:
                    return "Competent";
                case 4:
                    return "Expert";
                case 5:
                    return "Master";
                case 6:
                    return "Dangerous";
                case 7:
                    return "Deadly";
                case 8:
                    return "Elite";
                default:
                    return "Unknown";
            }
        }

        public static string GetTradeRankVerbose(int tradeRank)
        {
            switch (tradeRank)
            {
                case 0:
                    return "Penniless";
                case 1:
                    return "Mostly Penniless";
                case 2:
                    return "Peddler";
                case 3:
                    return "Dealer";
                case 4:
                    return "Merchant";
                case 5:
                    return "Broker";
                case 6:
                    return "Entrepreneur";
                case 7:
                    return "Tycoon";
                case 8:
                    return "Elite";
                default:
                    return "Unknown";
            }
        }

        public static string GetExplorationRankVerbose(int explorationRank)
        {
            switch (explorationRank)
            {
                case 0:
                    return "Aimless";
                case 1:
                    return "Mostly Aimless";
                case 2:
                    return "Scout";
                case 3:
                    return "Surveyor";
                case 4:
                    return "Explorer";
                case 5:
                    return "Pathfinder";
                case 6:
                    return "Ranger";
                case 7:
                    return "Pioneer";
                case 8:
                    return "Elite";
                default:
                    return "Unknown";
            }
        }

        public static string GetCqcRankVerbose(int cqcRank)
        {
            switch (cqcRank)
            {
                case 0:
                    return "Helpless";
                case 1:
                    return "Mostly Helpless";
                case 2:
                    return "Amateur";
                case 3:
                    return "Semi Professional";
                case 4:
                    return "Professional";
                case 5:
                    return "Champion";
                case 6:
                    return "Hero";
                case 7:
                    return "Legend";
                case 8:
                    return "Elite";
                default:
                    return "Unknown";
            }
        }

        public static string GetFederationRankVerbose(int fedRank)
        {
            switch (fedRank)
            {
                case 0:
                    return "None";
                case 1:
                    return "Recruit";
                case 2:
                    return "Cadet";
                case 3:
                    return "Midshipman";
                case 4:
                    return "Petty Officer";
                case 5:
                    return "Chief Petty Officer";
                case 6:
                    return "Warrant Officer";
                case 7:
                    return "Ensign";
                case 8:
                    return "Lieutenant";
                case 9:
                    return "Lt. Commander";
                case 10:
                    return "Post Commander";
                case 11:
                    return "Post Captain";
                case 12:
                    return "Rear Admiral";
                case 13:
                    return "Vice Admiral";
                case 14:
                    return "Admiral";
                default:
                    return "Unknown";
            }
        }

        public static string GetEmpireRankVerbose(int empireRank)
        {
            switch (empireRank)
            {
                case 0:
                    return "None";
                case 1:
                    return "Outsider";
                case 2:
                    return "Serf";
                case 3:
                    return "Master";
                case 4:
                    return "Squire";
                case 5:
                    return "Knight";
                case 6:
                    return "Lord";
                case 7:
                    return "Baron";
                case 8:
                    return "Viscount";
                case 9:
                    return "Count";
                case 10:
                    return "Earl";
                case 11:
                    return "Marquis";
                case 12:
                    return "Duke";
                case 13:
                    return "Prince";
                case 14:
                    return "King";
                default:
                    return "Unknown";
            }
        }
    }
}
