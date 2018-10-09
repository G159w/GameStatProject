using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;

namespace DotnetProjectUwp.ViewModels
{
    public class ViewModelNavigation
    {
        public ViewModelBase CurrentViewModel { get; set; }
        public Page CurrentPage { get; set; }

        private readonly Stack<Page> _stackPages;

        public ViewModelNavigation()
        {
            _stackPages = new Stack<Page>();
        }

        public void LoadPage(Page newPage)
        {
            if (CurrentPage != null)
            {
                _stackPages.Push(CurrentPage);
            }
            CurrentPage = newPage;
            CurrentViewModel.RaisePropertyChanged("Page");
        }

        public void Return()
        {
            if (_stackPages.Any())
            {
                CurrentPage = _stackPages.Pop();
                CurrentViewModel.RaisePropertyChanged("Page");
            }
        }

        public void NewContext()
        {
            _stackPages.Clear();
        }
    }
}
