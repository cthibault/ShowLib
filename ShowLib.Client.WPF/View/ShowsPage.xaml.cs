using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows;
using ShowLib.Client.WPF.ViewModel;
using ShowLib.Data.Repositories;

namespace ShowLib.Client.WPF.View
{
    /// <summary>
    /// Interaction logic for ShowsPage.xaml
    /// </summary>
    public partial class ShowsPage : UserControl, IContent
    {
        public ShowsPage()
        {
            InitializeComponent();
        }

        #region IContent Methods
        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            //DO NOTHING
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            //DO NOTHING
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            this.ViewModel.GetShows();
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //DO NOTHING
        } 
        #endregion

        #region Private Properties
        private ShowsViewModel ViewModel
        {
            get
            {
                if (this._viewModel == null)
                {
                    var context = new ShowLibContext(null);
                    this._viewModel = new ShowsViewModel(context);

                    this.DataContext = this._viewModel;
                }
                return this._viewModel;
            }
        }
        #endregion

        #region Private Fields
        private ShowsViewModel _viewModel;
        #endregion
    }
}
