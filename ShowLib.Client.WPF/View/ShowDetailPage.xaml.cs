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

namespace ShowLib.Client.WPF.View
{
    /// <summary>
    /// Interaction logic for ShowDetail.xaml
    /// </summary>
    public partial class ShowDetailPage : UserControl, IContent
    {
        public ShowDetailPage()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            var vm = this.DataContext as ShowsViewModel;

            if (vm != null)
            {
                int showId = 0;
                if (int.TryParse(e.Fragment, out showId))
                {
                    vm.SelectShow(showId);
                }
            }
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            //DO NOTHING
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            //DO NOTHING
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //DO NOTHING
        }
    }
}
