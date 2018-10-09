using System;
using DotnetProjectBack.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotnetProjectBack.DatabaseModels
{
    public partial class DatabaseContext : DbContext
    {
        public virtual DbSet<TEliteStat> TEliteStat { get; set; }
        public virtual DbSet<TFortnite> TFortnite { get; set; }
        public virtual DbSet<TFriend> TFriend { get; set; }
        public virtual DbSet<TGame> TGame { get; set; }
        public virtual DbSet<TGw2Stat> TGw2Stat { get; set; }
        public virtual DbSet<TLolStat> TLolStat { get; set; }
        public virtual DbSet<TR6Stat> TR6Stat { get; set; }
        public virtual DbSet<TUser> TUser { get; set; }
        public virtual DbSet<TUserGame> TUserGame { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEliteStat>(entity =>
            {
                entity.ToTable("T_Elite_Stat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CombatProgress).HasColumnName("combat_progress");

                entity.Property(e => e.CombatRank).HasColumnName("combat_rank");

                entity.Property(e => e.CqcProgress).HasColumnName("cqc_progress");

                entity.Property(e => e.CqcRank).HasColumnName("cqc_rank");

                entity.Property(e => e.EmpireProgress).HasColumnName("empire_progress");

                entity.Property(e => e.EmpireRank).HasColumnName("empire_rank");

                entity.Property(e => e.ExplorerProgress).HasColumnName("explorer_progress");

                entity.Property(e => e.ExplorerRank).HasColumnName("explorer_rank");

                entity.Property(e => e.FederationProgress).HasColumnName("federation_progress");

                entity.Property(e => e.FederationRank).HasColumnName("federation_rank");

                entity.Property(e => e.TraderProgress).HasColumnName("trader_progress");

                entity.Property(e => e.TraderRank).HasColumnName("trader_rank");

                entity.Property(e => e.UserGameId).HasColumnName("user_game_id");

                entity.HasOne(d => d.UserGame)
                    .WithMany(p => p.TEliteStat)
                    .HasForeignKey(d => d.UserGameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_Elite_Stat_T_User_Game");
            });

            modelBuilder.Entity<TFortnite>(entity =>
            {
                entity.ToTable("T_Fortnite");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DuoTop1).HasColumnName("duoTop1");

                entity.Property(e => e.DuoTop12).HasColumnName("duoTop12");

                entity.Property(e => e.DuoTop5).HasColumnName("duoTop5");

                entity.Property(e => e.Kd).HasColumnName("kd");

                entity.Property(e => e.Kills).HasColumnName("kills");

                entity.Property(e => e.Matches).HasColumnName("matches");

                entity.Property(e => e.SoloTop1).HasColumnName("soloTop1");

                entity.Property(e => e.SoloTop10).HasColumnName("soloTop10");

                entity.Property(e => e.SoloTop25).HasColumnName("soloTop25");

                entity.Property(e => e.SquadTop1).HasColumnName("squadTop1");

                entity.Property(e => e.SquadTop3).HasColumnName("squadTop3");

                entity.Property(e => e.SquadTop6).HasColumnName("squadTop6");

                entity.Property(e => e.UserGameId).HasColumnName("user_game_id");

                entity.Property(e => e.WinPercent).HasColumnName("winPercent");

                entity.Property(e => e.Wins).HasColumnName("wins");

                entity.HasOne(d => d.UserGame)
                    .WithMany(p => p.TFortnite)
                    .HasForeignKey(d => d.UserGameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_Fortnite_T_User_Game");
            });

            modelBuilder.Entity<TFriend>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FriendId });

                entity.ToTable("T_Friend");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.FriendId).HasColumnName("friend_id");

                entity.HasOne(d => d.Friend)
                    .WithMany(p => p.TFriendFriend)
                    .HasForeignKey(d => d.FriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_Friend_T_User1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TFriendUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_Friend_T_User");
            });

            modelBuilder.Entity<TGame>(entity =>
            {
                entity.ToTable("T_Game");

                entity.HasIndex(e => e.DisplayName)
                    .HasName("IX_T_Game_1")
                    .IsUnique();

                entity.HasIndex(e => e.ShortName)
                    .HasName("IX_T_Game")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApiKeyRequired).HasColumnName("api_key_required");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasColumnName("display_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasColumnName("short_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TGw2Stat>(entity =>
            {
                entity.ToTable("T_Gw2_Stat");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Byes).HasColumnName("byes");

                entity.Property(e => e.Desertions).HasColumnName("desertions");

                entity.Property(e => e.Forfeits).HasColumnName("forfeits");

                entity.Property(e => e.Losses).HasColumnName("losses");

                entity.Property(e => e.PvpRank).HasColumnName("pvp_rank");

                entity.Property(e => e.PvpRankPoints).HasColumnName("pvp_rank_points");

                entity.Property(e => e.PvpRankRollovers).HasColumnName("pvp_rank_rollovers");

                entity.Property(e => e.UserGameId).HasColumnName("user_game_id");

                entity.Property(e => e.Wins).HasColumnName("wins");

                entity.HasOne(d => d.UserGame)
                    .WithMany(p => p.TGw2Stat)
                    .HasForeignKey(d => d.UserGameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_Gw2_Stat_T_User_Game");
            });

            modelBuilder.Entity<TLolStat>(entity =>
            {
                entity.ToTable("T_Lol_Stat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FlexLosses).HasColumnName("flexLosses");

                entity.Property(e => e.FlexLp).HasColumnName("flexLP");

                entity.Property(e => e.FlexNameLeague)
                    .IsRequired()
                    .HasColumnName("flexNameLeague")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FlexRank)
                    .IsRequired()
                    .HasColumnName("flexRank")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FlexTier)
                    .IsRequired()
                    .HasColumnName("flexTier")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FlexWins).HasColumnName("flexWins");

                entity.Property(e => e.SoloLosses).HasColumnName("soloLosses");

                entity.Property(e => e.SoloLp).HasColumnName("soloLP");

                entity.Property(e => e.SoloNameLeague)
                    .IsRequired()
                    .HasColumnName("soloNameLeague")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SoloRank)
                    .IsRequired()
                    .HasColumnName("soloRank")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SoloTier)
                    .IsRequired()
                    .HasColumnName("soloTier")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SoloWins).HasColumnName("soloWins");

                entity.Property(e => e.UserGameId).HasColumnName("user_game_id");

                entity.HasOne(d => d.UserGame)
                    .WithMany(p => p.TLolStat)
                    .HasForeignKey(d => d.UserGameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_Lol_Stat_T_User_Game");
            });

            modelBuilder.Entity<TR6Stat>(entity =>
            {
                entity.ToTable("T_R6_Stat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CasualDeaths).HasColumnName("casual_deaths");

                entity.Property(e => e.CasualKd).HasColumnName("casual_kd");

                entity.Property(e => e.CasualKills).HasColumnName("casual_kills");

                entity.Property(e => e.CasualLosses).HasColumnName("casual_losses");

                entity.Property(e => e.CasualPlaytime)
                    .IsRequired()
                    .HasColumnName("casual_playtime")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CasualWins).HasColumnName("casual_wins");

                entity.Property(e => e.CasualWlr).HasColumnName("casual_wlr");

                entity.Property(e => e.PlayerLevel).HasColumnName("player_level");

                entity.Property(e => e.RankedDeaths).HasColumnName("ranked_deaths");

                entity.Property(e => e.RankedKd).HasColumnName("ranked_kd");

                entity.Property(e => e.RankedKills).HasColumnName("ranked_kills");

                entity.Property(e => e.RankedLosses).HasColumnName("ranked_losses");

                entity.Property(e => e.RankedPlaytime)
                    .IsRequired()
                    .HasColumnName("ranked_playtime")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RankedWins).HasColumnName("ranked_wins");

                entity.Property(e => e.RankedWlr).HasColumnName("ranked_wlr");

                entity.Property(e => e.UserGameId).HasColumnName("user_game_id");

                entity.HasOne(d => d.UserGame)
                    .WithMany(p => p.TR6Stat)
                    .HasForeignKey(d => d.UserGameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_R6_Stat_T_User_Game");
            });

            modelBuilder.Entity<TUser>(entity =>
            {
                entity.ToTable("T_User");

                entity.HasIndex(e => e.Email)
                    .HasName("IX_T_User")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("IX_T_User_1")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("password_salt")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RegisterDate)
                    .HasColumnName("register_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TUserGame>(entity =>
            {
                entity.ToTable("T_User_Game");

                entity.HasIndex(e => new { e.GameId, e.Username })
                    .HasName("IX_T_User_Game")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApiKey)
                    .HasColumnName("api_key")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.TUserGame)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_User_Game_T_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TUserGame)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_User_Game_T_User");
            });
        }
    }
}