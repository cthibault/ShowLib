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
using ShowLib.Data.Entities;
using ShowLib.Data.Repositories;

namespace ShowLib.Client.WPF.ViewModel
{
    public class TvdbSearchViewModel : ReactiveObject
    {
        public TvdbSearchViewModel()
        {
            this.InitializeCommands();
        }

        #region Private Methods

        private void InitializeCommands()
        {
            this.SearchCommand = new ReactiveCommand(this.WhenAny(vm => vm.SearchText, x => !string.IsNullOrEmpty(x.Value)));
            this.SearchCommand.Subscribe(x => this.OnSearch());
        }

        #endregion

        #region Commands

        public ReactiveCommand SearchCommand { get; set; }

        #region Command Handlers

        private async Task OnSearch()
        {
            this.SearchResults.Clear();

            try
            {
                var results = await this.TvdbRepository.SearchSeries(this.SearchText.Trim());

                if (results != null)
                {
                    foreach (var result in results)
                    {
                        this.SearchResults.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #endregion

        #region Public Properties

        public string SearchText 
        {
            get { return this._searchText; }
            set { this.RaiseAndSetIfChanged(ref this._searchText, value); }
        }

        public ObservableCollection<TvdbSearchResult> SearchResults
        {
            get
            {
                if (this._searchResults == null)
                {
                    this.RaiseAndSetIfChanged(ref this._searchResults, new ObservableCollection<TvdbSearchResult>());
                }
                return this._searchResults;
            }
        }

        public TvdbSearchResult SelectedSearchResult
        {
            get { return this._selectedSearchResult; }
            set { this.RaiseAndSetIfChanged(ref this._selectedSearchResult, value); }
        }

        #endregion

        #region Private Properties
        private ITvdbRepository TvdbRepository
        {
            get
            {
                if (this._tvdbRepository == null)
                {
                    this._tvdbRepository = new TvdbRepository();
                }
                return this._tvdbRepository;
            }
        }
        #endregion

        #region Private Fields
        private ITvdbRepository _tvdbRepository;
        private string _searchText;
        private ObservableCollection<TvdbSearchResult> _searchResults;
        private TvdbSearchResult _selectedSearchResult;
        #endregion
    }
}
