using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using ReactiveUI;
using ShowLib.Client.WPF.Model;
using ShowLib.Data.Entities;
using ShowLib.Data.Repositories;

namespace ShowLib.Client.WPF.ViewModel
{
    public class ShowsViewModel : ReactiveObject
    {
        public ShowsViewModel(IShowLibContext context)
        {
            this.Context = context;

            this.InitializeCommands();
        }
        
        #region Public Methods

        public async Task GetShows()
        {
            this.SaveCurrent();

            var shows = await this.Context.ShowRepository.LoadAll();

            foreach (var show in shows)
            {
                this.AddShow(show);
            }
        }

        public async Task SelectShow(int showId)
        {
            this.SaveCurrent();

            Show show = null;

            if (showId > 0)
            {
                show = this.Shows.Single(s => s.Id == showId);

                show = await this.Context.ShowRepository.Load(show.Id);
            }

            this.ActiveShow = show;
        }

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            this.BrowseDirectoryCommand = new ReactiveCommand();
            this.BrowseDirectoryCommand.Subscribe(x => this.OnBrowseDirectory());

            this.SearchTvdbCommand = new ReactiveCommand();
            this.SearchTvdbCommand.Subscribe(x => this.OnSearchTvdb());

            this.AddNewShowCommand = new ReactiveCommand();
            this.AddNewShowCommand.Subscribe(x => this.OnAddNewShow());

            this.DeleteShowCommand = new ReactiveCommand(this.WhenAny(vm => vm.ActiveShow, x => x.Value != null));
            this.DeleteShowCommand.Subscribe(x => this.OnDeleteShow());
        }

        private void AddShow(Show show)
        {
            if (show != null)
            {
                if (!this.Shows.Any(s => s.Id == show.Id))
                {
                    this.Shows.Add(show);
                    this.Links.Add(new Link
                    {
                        DisplayName = show.Title, 
                        Source = new Uri("/view/ShowDetail.xaml#" + show.Id, UriKind.Relative) 
                    });
                }
            }
        }
        private void RemoveShow(Show show)
        {
            if (show != null)
            {

            }
        }

        private async Task OnAddNewShow()
        {
            var show = new Show 
            {
                Title = "New Show",
                ShowDetail = new ShowDetail()
            };

            var result = await this.Context.ShowRepository.Save(show);

            this.AddShow(show);

            this.SaveCurrent();

            this.SelectShow(show.Id);
        }

        private async Task OnDeleteShow()
        {
            if (this.ActiveShow != null)
            {
                MessageBoxButton buttons = MessageBoxButton.YesNo;

                string message = string.Format("Are you sure you want to delete '{0}'?", this.ActiveShow.Title);

                var result = ModernDialog.ShowMessage(message, "Delete Show", buttons);

                if (result == MessageBoxResult.Yes)
                {
                    var deleteResult = await this.Context.ShowRepository.Delete(this.ActiveShow.Id);

                    if (deleteResult)
                    {
                        var show = this.ActiveShow;

                        this.ActiveShow = null;

                        this.RemoveShow(show);
                    }
                }
            }
        }

        private void SaveCurrent()
        {
            if (this.ActiveShow != null)
            {
                this.Context.ShowRepository.Save(this.ActiveShow);
            }
        }

        private void OnBrowseDirectory()
        {
            if (this.ActiveShow != null && this.ActiveShow.ShowDetail != null)
            {
                var dialog = new FolderBrowserDialog
                {
                    ShowNewFolderButton = true,
                    Description = "Select Series Directory",
                    RootFolder = Environment.SpecialFolder.MyComputer,
                    SelectedPath = this.ActiveShow.ShowDetail.Directory
                };

                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.ActiveShow.ShowDetail.Directory = dialog.SelectedPath;
                }
            }
        }

        private void OnSearchTvdb()
        {

        }

        #endregion

        #region Commands

        public ReactiveCommand BrowseDirectoryCommand { get; set; }
        public ReactiveCommand SearchTvdbCommand { get; set; }
        public ReactiveCommand AddNewShowCommand { get; set; }
        public ReactiveCommand DeleteShowCommand { get; set; }

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
            set { this.RaiseAndSetIfChanged(ref this._activeShow, value); }
        }

        #endregion

        #region Private Properties
        private IShowLibContext Context { get; set; }
        #endregion

        #region Private Fields
        private IShowRepository _showRepository;
        private LinkCollection _links;
        private ObservableCollection<Show> _shows;
        private Show _activeShow;
        #endregion
    }
}
