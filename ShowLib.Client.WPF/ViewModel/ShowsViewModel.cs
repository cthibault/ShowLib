using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight;
using ShowLib.Data.Entity;
using ShowLib.Data.Repository;

namespace ShowLib.Client.WPF.ViewModel
{
    public class ShowsViewModel : ViewModelBase
    {
        public ShowsViewModel()
        {
        }

        #region Public Methods
        public async Task GetShows()
        {
            var shows = await this.ShowRepository.GetShows();

            foreach (var show in shows)
            {
                this.AddShow(show);
            }
        }

        public async Task GetShowDetails(int showId)
        {
            if (showId > 0)
            {
                var show = this.Shows.SingleOrDefault(s => s.Id == showId);

                if (show != null)
                {
                    var show2 = await this.ShowRepository.GetShowDetail(show);

                    show.ShowDetail = show2.ShowDetail;

                    this.ActiveShow = show;
                }
            }
        }
        #endregion

        #region Private Methods

        private void AddShow(Show show)
        {
            if (show != null)
            {
                if (!this.Shows.Any(s => s.Id == show.Id))
                {
                    this.Shows.Add(show);
                    this.Links.Add(new Link { DisplayName = show.Title, Source = new Uri("/view/ShowDetail.xaml#" + show.Id, UriKind.Relative) });

                }
            }
        }
        private void RemoveShow(Show show)
        {
            if (show != null)
            {
                
            }
        }

        private bool ResolveUnsavedChanges()
        {
            return false;
        }

        #endregion

        #region Public Properties

        public LinkCollection Links
        {
            get
            {
                if (this._links == null)
                {
                    this._links = new LinkCollection();
                }
                return this._links;
            }
        }

        public ObservableCollection<Show> Shows
        {
            get
            {
                if (this._shows == null)
                {
                    this._shows = new ObservableCollection<Show>();
                }
                return this._shows;
            }
        }

        public Show ActiveShow
        {
            get { return this._activeShow; }
            set
            {
                if (this._activeShow != value)
                {
                    this._activeShow = value;
                    this.RaisePropertyChanged(() => this.ActiveShow);
                }
            }
        }

        #endregion

        #region Private Properties
        private IShowRepository ShowRepository
        {
            get
            {
                if (this._showRepository == null)
                {
                    var repo = new MocksShowRepository();
                    repo.LongRunningProcess = () => Task.Delay(5000);

                    this._showRepository = repo;
                }

                return this._showRepository;
            }
        }
        #endregion

        #region Private Fields
        private IShowRepository _showRepository;
        private LinkCollection _links;
        private ObservableCollection<Show> _shows;
        private Show _activeShow;
        #endregion
    }
}
