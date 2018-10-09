using CommonServiceLocator;
using DotnetProjectUwp.ViewModels;
using DotnetProjectUwp.ViewModels.Games;
using DotnetProjectUwp.ViewModels.Leaderboard;
using DotnetProjectUwp.ViewModels.Profile;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Models.Responses;
using RestSharp;

namespace DotNetProjectUwp.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary> 
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }
            else
            {
                // Create run time view services and models
            }

            //Register your services used here
            var nav = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<ViewModelNavigation>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ConnexionViewModel>();
            SimpleIoc.Default.Register<InscriptionViewModel>();
            SimpleIoc.Default.Register<LeaderboardsViewModel>();
            SimpleIoc.Default.Register<EliteViewModel>();
            SimpleIoc.Default.Register<EliteLeaderboardViewModel>();
            SimpleIoc.Default.Register<R6ViewModel>();
            SimpleIoc.Default.Register<R6LeaderboardViewModel>();
            SimpleIoc.Default.Register<LolViewModel>();
            SimpleIoc.Default.Register<LolLeaderboardViewModel>();
            SimpleIoc.Default.Register<FortniteViewModel>();
            SimpleIoc.Default.Register<FortniteLeaderboardViewModel>();
            SimpleIoc.Default.Register<Gw2ViewModel>();
            SimpleIoc.Default.Register<Gw2LeaderboardViewModel>();
            SimpleIoc.Default.Register<ProfileViewModel>();
            SimpleIoc.Default.Register<FriendsViewModel>();
            SimpleIoc.Default.Register<CompareViewModel>();
            SimpleIoc.Default.Register<RestClient>(() => new RestClient("http://localhost:57089/api"));
        }


        // <summary>
        // Gets the StartPageKey view model.
        // </summary>
        // <value>
        // The StartPageKey view model.
        // </value>
        public SearchViewModel SearchInstance => ServiceLocator.Current.GetInstance<SearchViewModel>();
        public MainViewModel MainInstance => ServiceLocator.Current.GetInstance<MainViewModel>();
        public ConnexionViewModel ConnexionInstance => ServiceLocator.Current.GetInstance<ConnexionViewModel>();
        public LeaderboardsViewModel LeaderboardsInstance => ServiceLocator.Current.GetInstance<LeaderboardsViewModel>();
        public EliteViewModel EliteInstance => ServiceLocator.Current.GetInstance<EliteViewModel>();
        public EliteLeaderboardViewModel EliteLeaderboardInstace => ServiceLocator.Current.GetInstance<EliteLeaderboardViewModel>();
        public R6ViewModel R6Instance => ServiceLocator.Current.GetInstance<R6ViewModel>();
        public R6LeaderboardViewModel R6LeaderboardInstance => ServiceLocator.Current.GetInstance<R6LeaderboardViewModel>();
        public ProfileViewModel ProfileInstance => ServiceLocator.Current.GetInstance<ProfileViewModel>();
        public InscriptionViewModel InscriptionInstance => ServiceLocator.Current.GetInstance<InscriptionViewModel>();
        public FriendsViewModel FriendInstance => ServiceLocator.Current.GetInstance<FriendsViewModel>();
        public LolViewModel LolInstance => ServiceLocator.Current.GetInstance<LolViewModel>();
        public LolLeaderboardViewModel LolLeaderboardInstance => ServiceLocator.Current.GetInstance<LolLeaderboardViewModel>();
        public FortniteViewModel ForniteInstance => ServiceLocator.Current.GetInstance<FortniteViewModel>();
        public FortniteLeaderboardViewModel FortniteLeaderboardInstance =>
            ServiceLocator.Current.GetInstance<FortniteLeaderboardViewModel>();
        public Gw2ViewModel Gw2Instance => ServiceLocator.Current.GetInstance<Gw2ViewModel>();
        public Gw2LeaderboardViewModel Gw2LeaderboardInstance => ServiceLocator.Current.GetInstance<Gw2LeaderboardViewModel>();
        public CompareViewModel CompareInstance => ServiceLocator.Current.GetInstance<CompareViewModel>();


        // <summary>
        // The cleanup.
        // </summary>
        public static void Cleanup()
        {
        }
    }

}
