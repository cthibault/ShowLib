using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;

namespace ShowLib.Data.Repositories
{
    public class ShowLibContext : IShowLibContext
    {
        public ShowLibContext(string url)
        {
            this.Url = url;
        }

        #region Repositories
        public IShowRepository ShowRepository
        {
            get
            {
                if (this._showRepository == null)
                {
                    this._showRepository = new ShowRepository(this.DocumentStore);
                }
                return this._showRepository;
            }
        }
        #endregion

        #region Public Properties
        public string Url
        {
            get { return this._url; }
            private set
            {
                if (this._url != value)
                {
                    if (this._documentStore != null)
                    {
                        this._documentStore.Dispose();
                        this._documentStore = null;
                    }

                    this._url = value;
                }
            }
        }
        #endregion

        #region Private Properties
        private DocumentStore DocumentStore
        {
            get
            {
                if (this._documentStore == null)
                {
                    string url = !string.IsNullOrEmpty(this.Url) ? this.Url : @"http://localhost:8080/databases/ShowLibrary";

                    this._documentStore = new DocumentStore() { Url = url };

                    this._documentStore.Initialize();
                }

                return this._documentStore;
            }
        }
        #endregion

        #region Private Fields
        private string _url;
        private DocumentStore _documentStore;

        private IShowRepository _showRepository;
        #endregion
    }
}
