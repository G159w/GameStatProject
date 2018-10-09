using System;
using System.Security.Claims;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using DotnetProjectUwp.ViewModels;
using DotnetProjectUwp.Views;
using DotnetProjectUwp.Views.Profile;
using DotNetProjectUwp.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace DotNetProjectUwp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ViewModelNavigation _viewModelNavigation;

        private bool _isMenuOpen;
        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set
            {
                _isMenuOpen = value;
                RaisePropertyChanged();
            }
        }

        private ICommand _openCloseMenuCommand;
        public ICommand OpenCloseMenuCommand
        {
            get { return _openCloseMenuCommand = _openCloseMenuCommand ?? new RelayCommand(OpenCloseMenuExecute); }
        }
        private void OpenCloseMenuExecute()
        {
            IsMenuOpen = !IsMenuOpen;
        }

        public Page Page
        {
            get => _viewModelNavigation.CurrentPage;
            set => _viewModelNavigation.LoadPage(value);
        }


        private RelayCommand<string> _navigationCommand;

        public MainViewModel(ViewModelNavigation viewModelNavigation)
        {
            _viewModelNavigation = viewModelNavigation;
            _viewModelNavigation.CurrentViewModel = this;
            Page = new SearchPage();
        }

        public RelayCommand<string> NavigationCommand
        {
            get { return _navigationCommand = _navigationCommand ?? new RelayCommand<string>(NavigationExecute); }
        }

        public ICommand Return
        {
            get { return new RelayCommand(() => _viewModelNavigation.Return()); }
        }

        private void NavigationExecute(string viewFrame)
        {
            switch (viewFrame)
            {   
                case "SearchPage":
                    Page = new SearchPage();
                    break;
                case "ConnexionPage":
                    if (ClaimsPrincipal.Current != null && ClaimsPrincipal.Current.Identity.IsAuthenticated)
                    {
                        Page = new ProfilePage();   
                    }
                    else
                    {
                        Page = new ConnexionPage();
                    }
                    break;
                case "Leaderboard":
                    Page = new LeaderboardPage();
                    break;
                case "ComparePage":
                    Page = new ComparePage();
                    break;
            }
            _viewModelNavigation.NewContext();
            IsMenuOpen = false;
        }

    }
}