using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using ShowLib.Core;

namespace ShowLib.Data.Entity
{
    public class ShowDetail : BaseObservableModel, IObjectChangeTracking
    {
        #region Private Methods
        private void OnParsers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.OfType<IObjectChangeTracking>())
                    {
                        item.TrackChanges = this.TrackChanges;
                        item.StateChanged += change =>
                        {
                            if (this.TrackChanges)
                            {
                                this.HasChanged = change;
                            }
                        };
                    }
                    break;
            }

            if (this.TrackChanges)
            {
                this.HasChanged = true;
            }
        }
        #endregion

        #region Public Properties
        public int TvdbId
        {
            get { return this._tvdbId; }
            set
            {
                if (this._tvdbId != value)
                {
                    this._tvdbId = value;
                    this.RaisePropertyChanged(() => this.TvdbId);

                    if (this.TrackChanges)
                    {
                        this.HasChanged = true;
                    }
                }
            }
        }
        public string ImdbId
        {
            get { return this._imdbId; }
            set
            {
                if (this._imdbId != value)
                {
                    this._imdbId = value;
                    this.RaisePropertyChanged(() => this.ImdbId);

                    if (this.TrackChanges)
                    {
                        this.HasChanged = true;
                    }
                }
            }
        }
        public string Directory
        {
            get { return this._directory; }
            set
            {
                if (this._directory != value)
                {
                    this._directory = value;
                    this.RaisePropertyChanged(() => this.Directory);

                    if (this.TrackChanges)
                    {
                        this.HasChanged = true;
                    }
                }
            }
        }

        public ObservableCollection<Parser> Parsers
        {
            get
            {
                if (this._parsers == null)
                {
                    this._parsers = new ObservableCollection<Parser>();
                    this._parsers.CollectionChanged += this.OnParsers_CollectionChanged;
                    this.RaisePropertyChanged(() => this.Parsers);
                }

                return this._parsers;
            }
        }
        #endregion

        #region IObjectChangeTracking
        public bool TrackChanges
        {
            get { return this._trackChanges; }
            set 
            { 
                if (this._trackChanges != value)
                {
                    this._trackChanges = value;

                    foreach (var parser in this.Parsers)
                    {
                        parser.TrackChanges = value;
                    }
                }
            }
        }

        public bool HasChanged
        {
            get { return this._hasChanged; }
            set
            {
                if (this._hasChanged != value)
                {
                    this._hasChanged = value;

                    if (this.StateChanged != null)
                    {
                        this.StateChanged(value);
                    }
                }
            }
        }

        public Action<bool> StateChanged { get; set; }
        #endregion

        #region Private Fields
        private int _tvdbId;
        private string _imdbId = string.Empty;
        private string _directory = string.Empty;

        private ObservableCollection<Parser> _parsers;

        private bool _trackChanges;
        private bool _hasChanged;
        #endregion
    }
}
