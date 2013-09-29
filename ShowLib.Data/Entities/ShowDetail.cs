using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using ShowLib.Core;

namespace ShowLib.Data.Entities
{
    public class ShowDetail : BaseObservableModel
    {
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
                }

                return this._parsers;
            }
        }
        #endregion

        #region Private Fields
        private int _tvdbId;
        private string _imdbId = string.Empty;
        private string _directory = string.Empty;

        private ObservableCollection<Parser> _parsers;
        #endregion
    }
}
