using System.Linq;
using Windows.UI.Xaml.Controls;
using CommonServiceLocator;
using DotNetProjectUwp.ViewModels;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace DotNetProjectUwp.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            // Only binding
            var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
            var nav = ServiceLocator.Current.GetInstance<MainViewModel>().NavigationCommand;
            nav.Execute(item.Tag);
        }

    }
}
