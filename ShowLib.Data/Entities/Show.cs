using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ShowLib.Core;

namespace ShowLib.Data.Entities
{
    public class Show : BaseObservableModel
    {
        #region Constructors
        public Show() { }

        public Show(int id)
        {
            this.Id = id;
        } 
        #endregion

        #region Public Properties
        public int Id { get; set; }
        
        public string Title
        {
            get { return this._title; }
            set
            {
                if (this._title != value)
                {
                    this._title = value;
                    this.RaisePropertyChanged(() => this.Title);
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
                    this._showDetail = value;
                    this.RaisePropertyChanged(() => this.ShowDetail);
                }
            }
        }
        #endregion

        #region Private Fields
        private string _title = string.Empty;
        private ShowDetail _showDetail;
        #endregion
    }
}
