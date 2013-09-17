using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ShowLib.Core;

namespace ShowLib.Data.Entity
{
    public class Show : BaseObservableModel, IObjectChangeTracking
    {
        #region Constructors
        public Show()
        {
            this.HasChanged = true;
        }
        internal Show(int id)
        {
            this.Id = id;
        } 
        #endregion

        #region Private Methods
        private void OnShowDetail_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.UpdateHasChanges(true);
        }
        private void UpdateHasChanges(bool change)
        {
            if (this.TrackChanges)
            {
                this.HasChanged = change;
            }
        }
        #endregion

        #region Public Properties
        public int Id { get; internal set; }
        
        public string Title
        {
            get { return this._title; }
            set
            {
                if (this._title != value)
                {
                    this._title = value;
                    this.RaisePropertyChanged(() => this.Title);

                    if (this.TrackChanges)
                    {
                        this.HasChanged = true;
                    }
                }
            }
        }

        public ShowDetail ShowDetail
        {
            get { return this._showDetail; }
            set
            {
                if (this._showDetail != value)
                {
                    if (this._showDetail != null)
                    {
                        this._showDetail.PropertyChanged -= this.OnShowDetail_PropertyChanged;
                    }

                    this._showDetail = value;
                    this._showDetail.TrackChanges = this.TrackChanges;
                    this._showDetail.PropertyChanged += this.OnShowDetail_PropertyChanged;
                    this._showDetail.StateChanged += this.UpdateHasChanges;

                    if (this.TrackChanges)
                    {
                        this.HasChanged = true;
                    }

                    this.RaisePropertyChanged(() => this.ShowDetail);
                }
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

                    if (this.ShowDetail != null)
                    {
                        this.ShowDetail.TrackChanges = value;
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
        private string _title = string.Empty;
        private ShowDetail _showDetail;

        private bool _trackChanges;
        private bool _hasChanged;
        #endregion
    }
}
