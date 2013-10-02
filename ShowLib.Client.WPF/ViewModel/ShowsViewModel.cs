using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using ReactiveUI;
using ShowLib.Client.WPF.Model;
using ShowLib.Client.WPF.View;
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
                var showLink = this.ShowLinks.OfType<ShowLink>().SingleOrDefault(sl => sl.Item.Id == showId);

                if (showLink != null)
                {
                    show = await this.Context.ShowRepository.Load(showLink.Item);
                    showLink.PopulateWith(show);

                    this.SelectedSource = showLink.Source;
                }
            }

            this.ActiveShow = show;
        }

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            this.BrowseDirectoryCommand = new ReactiveCommand(this.WhenAny(vm => vm.ActiveShow, x => x.Value != null && x.Value.ShowDetail != null));
            this.BrowseDirectoryCommand.Subscribe(x => this.OnBrowseDirectory());

            this.SearchTvdbCommand = new ReactiveCommand(this.WhenAny(vm => vm.ActiveShow, x => x.Value != null));
            this.SearchTvdbCommand.Subscribe(x => this.OnSearchTvdb());

            this.AddNewShowCommand = new ReactiveCommand();
            this.AddNewShowCommand.Subscribe(x => this.OnAddNewShow());

            this.DeleteShowCommand = new ReactiveCommand(this.WhenAny(vm => vm.ActiveShow, x => x.Value != null));
            this.DeleteShowCommand.Subscribe(x => this.OnDeleteShow());

            this.RefreshShowsCommand = new ReactiveCommand();
            this.RefreshShowsCommand.Subscribe(x => this.OnRefreshShows());
        }

        private void AddShow(Show show)
        {
            if (show != null)
            {
                if (!this.ShowLinks.OfType<ShowLink>().Any(sl => sl.Item.Id == show.Id))
                {
                    var showLink = new ShowLink(show);

                    this.ShowLinks.Add(showLink);
                }
            }
        }

        private void RemoveShow(Show show)
        {
            if (show != null)
            {
                var showLink = this.ShowLinks.OfType<ShowLink>().SingleOrDefault(sl => sl.Item.Id == show.Id);

                if (showLink != null)
                {
                    this.ShowLinks.Remove(showLink);

                    this.SelectedSource = null;

                    var newSelectedLink = this.ShowLinks.FirstOrDefault();
                    if (newSelectedLink != null)
                    {
                        this.SelectedSource = newSelectedLink.Source;
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

        #endregion

        #region Commands

        public ReactiveCommand AddNewShowCommand { get; set; }
        public ReactiveCommand DeleteShowCommand { get; set; }
        public ReactiveCommand RefreshShowsCommand { get; set; }
        public ReactiveCommand BrowseDirectoryCommand { get; set; }
        public ReactiveCommand SearchTvdbCommand { get; set; }

        #region Command Handlers

        private async Task OnAddNewShow()
        {
            var show = new Show
            {
                Title = "New Show",
                ShowDetail = new ShowDetail()
            };

            var result = await this.Context.ShowRepository.Save(show);

            this.AddShow(show);

            this.SelectShow(show.Id);
        }

        private async Task OnDeleteShow()
        {
            if (this.ActiveShow != null)
            {
                var result = ModernDialog.ShowMessage(string.Format("Are you sure you want to delete '{0}'?", this.ActiveShow.Title), "Delete Show", MessageBoxButton.YesNo);

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

        private async Task OnRefreshShows()
        {
            this.SaveCurrent();

            var currentShow = this.ActiveShow;

            this.ActiveShow = null;
            this.SelectedSource = null;

            this.ShowLinks.Clear();

            await this.GetShows();

            if (currentShow != null)
            {
                this.SelectShow(currentShow.Id);
            }
        }

        private void OnBrowseDirectory()
        {
            if (this.ActiveShow != null && this.ActiveShow.ShowDetail != null)
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog
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
            var searchTvdbDialog = new ModernDialog() { Title = "Search Tvdb" };

            var tvdbSearchControl = new TvdbSearchControl();
            searchTvdbDialog.Content = tvdbSearchControl;

            var okButton = searchTvdbDialog.OkButton;
            var okButtonCommand = new ReactiveCommand(tvdbSearchControl.ViewModel.WhenAny(vm => vm.SelectedSearchResult, x => x.Value != null));
            okButtonCommand.Subscribe(x => searchTvdbDialog.CloseCommand.Execute(x));
            okButton.Command = okButtonCommand;
            okButton.IsDefault = false;            

            searchTvdbDialog.Buttons = new System.Windows.Controls.Button[] { okButton, searchTvdbDialog.CancelButton };

            var result = searchTvdbDialog.ShowDialog2();

            if (result == MessageBoxResult.OK)
            {
                var selectedResult = tvdbSearchControl.ViewModel.SelectedSearchResult;
                if (selectedResult != null)
                {
                    if (this.ActiveShow.ShowDetail == null)
                    {
                        this.ActiveShow.ShowDetail = new ShowDetail();
                    }

                    if (this.ActiveShow.Title != selectedResult.Title)
                    {
                        var message = string.Format("Do you want to update the Title based on the Selected Result?\n\n'{0}' -> '{1}'", this.ActiveShow.Title, selectedResult.Title);

                        result = ModernDialog.ShowMessage(message, "Update Title", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            this.ActiveShow.Title = selectedResult.Title;
                        }
                    }

                    this.ActiveShow.ShowDetail.TvdbId = selectedResult.TvdbId;
                    this.ActiveShow.ShowDetail.ImdbId = selectedResult.ImdbId;
                }
            }
        }

        #endregion

        #endregion

        #region Public Properties

        public LinkCollection ShowLinks
        {
            get
            {
                if (this._showLinks == null)
                {
                    this._showLinks = new LinkCollection();
                }
                return this._showLinks;
            }
        }

        public Show ActiveShow
        {
            get { return this._activeShow; }
            set { this.RaiseAndSetIfChanged(ref this._activeShow, value); }
        }

        public Uri SelectedSource 
        {
            get { return _selectedSource; }
            set { this.RaiseAndSetIfChanged(ref _selectedSource, value); }
        }

        #endregion

        #region Private Properties
        private IShowLibContext Context { get; set; }
        #endregion

        #region Private Fields
        private IShowRepository _showRepository;
        private LinkCollection _showLinks;
        private Uri _selectedSource;        
        private Show _activeShow;
        #endregion
    }
}
