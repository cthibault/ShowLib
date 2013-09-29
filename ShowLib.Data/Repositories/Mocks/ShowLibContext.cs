using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;

namespace ShowLib.Data.Repositories
{
    public class MockShowLibContext : IShowLibContext
    {
        public MockShowLibContext(string url)
        {
            this.Url = url;
            this.ShowRepository = new MocksShowRepository();
        }
        
        #region Repositories
        public IShowRepository ShowRepository { get; private set; }
        #endregion

        #region public Properties
        public string Url { get; private set; }
        #endregion
    }
}
