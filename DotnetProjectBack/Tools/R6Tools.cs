using DotnetProjectBack.DatabaseModels;
using Models.Responses;
using System;
using System.Linq;
using DotnetProjectBack.Models.ApiResponses;

namespace DotnetProjectBack.Tools
{
    public static class R6Tools
    {
        public static Func<TR6Stat, object> GetSortingFunctions(string mode, string gameMode)
        {
            if (gameMode == "ranked")
            {
                if (mode == "losses")
                {
                    return s => s.RankedLosses;
                }
                else if (mode == "wlr")
                {
                    return s => s.RankedWlr;

                }
                else if (mode == "kills")
                {
                    return s => s.RankedKills;
                }
                else if (mode == "deaths")
                {
                    return s => s.RankedDeaths;
                }
                else if (mode == "kd")
                {
                    return s => s.RankedKd;
                }
                else if (mode == "playtime")
                {
                    return s => Int64.Parse(new String(s.RankedPlaytime.Where(Char.IsDigit).ToArray()));
                }
                else
                {
                    return s => s.RankedWins;
                }
            }
            else
            {
                if (mode == "losses")
                {
                    return s => s.CasualLosses;
                }
                else if (mode == "wlr")
                {
                    return s => s.CasualWlr;

                }
                else if (mode == "kills")
                {
                    return s => s.CasualKills;
                }
                else if (mode == "deaths")
                {
                    return s => s.CasualDeaths;
                }
                else if (mode == "kd")
                {
                    return s => s.CasualKd;
                }
                else if (mode == "playtime")
                {
                    return s => Int64.Parse(new String(s.CasualPlaytime.Where(Char.IsDigit).ToArray()));
                }
                else
                {
                    return s => s.CasualWins;
                }
            }
        }

        public static R6StatsResponse ConvertToR6StatsResponse(this TR6Stat r6DbStats)
        {
            if (r6DbStats == null)
            {
                return null;
            }

            R6StatsResponse res = new R6StatsResponse
            {
                Player = new R6StatsResponse.R6PlayerStats
                {
                    Stats = new R6StatsResponse.R6InnerStats
                    {
                        Casual = new R6StatsResponse.R6ModeStats
                        {
                            Deaths = r6DbStats.CasualDeaths,
                            Kd = r6DbStats.CasualKd,
                            Kills = r6DbStats.CasualKills,
                            Losses = r6DbStats.CasualLosses,
                            Playtime = r6DbStats.CasualPlaytime,
                            Wins = r6DbStats.CasualWins,
                            Wlr = r6DbStats.CasualWlr
                        },
                        Ranked = new R6StatsResponse.R6ModeStats
                        {
                            Deaths = r6DbStats.RankedDeaths,
                            Kd = r6DbStats.RankedKd,
                            Kills = r6DbStats.RankedKills,
                            Losses = r6DbStats.RankedLosses,
                            Playtime = r6DbStats.RankedPlaytime,
                            Wins = r6DbStats.RankedWins,
                            Wlr = r6DbStats.RankedWlr
                        },
                        Progression = new R6StatsResponse.R6ProgressionStats
                        {
                            Level = r6DbStats.PlayerLevel
                        }
                    }
                }
            };

            return res;
        }

        public static R6StatsResponse ConvertToR6StatsResponse(this R6TrackerResponse r6TrackerResponse)
        {
            if (r6TrackerResponse == null)
            {
                return null;
            }

            // Convert casual/ranked player time to string
            TimeSpan casualPlaytime = TimeSpan.FromSeconds(r6TrackerResponse.Player.Stats.Casual.Playtime);
            TimeSpan rankedPlaytime = TimeSpan.FromSeconds(r6TrackerResponse.Player.Stats.Ranked.Playtime);

            R6StatsResponse res = new R6StatsResponse
            {
                Player = new R6StatsResponse.R6PlayerStats
                {
                    Stats = new R6StatsResponse.R6InnerStats
                    {
                        Casual = new R6StatsResponse.R6ModeStats
                        {
                            Deaths = r6TrackerResponse.Player.Stats.Casual.Deaths,
                            Kd = r6TrackerResponse.Player.Stats.Casual.Kd,
                            Kills = r6TrackerResponse.Player.Stats.Casual.Kills,
                            Losses = r6TrackerResponse.Player.Stats.Casual.Losses,
                            Playtime = $"{(long) casualPlaytime.TotalHours} hours",
                            Wins = r6TrackerResponse.Player.Stats.Casual.Wins,
                            Wlr = r6TrackerResponse.Player.Stats.Casual.Wlr,
                        },
                        Ranked = new R6StatsResponse.R6ModeStats
                        {
                            Deaths = r6TrackerResponse.Player.Stats.Ranked.Deaths,
                            Kd = r6TrackerResponse.Player.Stats.Ranked.Kd,
                            Kills = r6TrackerResponse.Player.Stats.Ranked.Kills,
                            Losses = r6TrackerResponse.Player.Stats.Ranked.Losses,
                            Playtime = $"{(long)rankedPlaytime.TotalHours} hours",
                            Wins = r6TrackerResponse.Player.Stats.Ranked.Wins,
                            Wlr = r6TrackerResponse.Player.Stats.Ranked.Wlr,
                        },
                        Progression = new R6StatsResponse.R6ProgressionStats
                        {
                            Level = r6TrackerResponse.Player.Stats.Progression.Level
                        }
                    }
                }
            };

            return res;
        }
    }
}
