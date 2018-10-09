using System;
using System.Collections.Generic;
using System.Linq;
using DotnetProjectUwp.Views.Leaderboard;
using DotNetProjectUwp.Views;
using GalaSoft.MvvmLight;

namespace DotNetProjectUwp.ViewModels
{
    public class LeaderboardsViewModel : ViewModelBase
    {
        private EliteLeaderboardPage _eliteLeaderboardPage;

        public EliteLeaderboardPage EliteLeaderboard
        {
            get => new EliteLeaderboardPage();
            set
            {
                _eliteLeaderboardPage = value;
                RaisePropertyChanged();
            }
        }

        private R6LeaderboardPage _r6LeaderboardPage;

        public R6LeaderboardPage R6LeaderboardPage
        {
            get => new R6LeaderboardPage();
            set
            {
                _r6LeaderboardPage = value;
                RaisePropertyChanged();
            }
        }

        private LolLeaderboardPage _lolLeaderboardPage;

        public LolLeaderboardPage LolLeaderboardPage
        {
            get => new LolLeaderboardPage();
            set
            {
                _lolLeaderboardPage = value;
                RaisePropertyChanged();
            }
        }

        private ForniteLeaderboardPage _forniteLeaderboardPage;

        public ForniteLeaderboardPage FortniteLeaderboardPage
        {
            get => new ForniteLeaderboardPage();
            set
            {
                _forniteLeaderboardPage = value;
                RaisePropertyChanged();
            }
        }

        private Gw2LeaderboardPage _gw2Leaderboard;
        public Gw2LeaderboardPage Gw2LeaderboardPage
        {
            get => new Gw2LeaderboardPage();
            set
            {
                _gw2Leaderboard = value;
                RaisePropertyChanged();
            }
        }

        public LeaderboardsViewModel()
        {
        }
    }
}