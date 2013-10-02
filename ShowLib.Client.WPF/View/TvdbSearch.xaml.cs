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
using ShowLib.Client.WPF.ViewModel;

namespace ShowLib.Client.WPF.View
{
    /// <summary>
    /// Interaction logic for TvdbSearch.xaml
    /// </summary>
    public partial class TvdbSearchControl : UserControl
    {
        public TvdbSearchControl()
        {
            InitializeComponent();

            this.DataContext = this.ViewModel;
        }

        #region Private Properties
        public TvdbSearchViewModel ViewModel
        {
            get
            {
                if (this._viewModel == null)
                {
                    this._viewModel = new TvdbSearchViewModel();
                }
                return this._viewModel;
            }
        }
        #endregion

        #region Private Fields
        private TvdbSearchViewModel _viewModel;
        #endregion
    }
}
