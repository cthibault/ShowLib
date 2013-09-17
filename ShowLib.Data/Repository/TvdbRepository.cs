using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowLib.Data.Entity;

namespace ShowLib.Data.Repository
{
    public class TvdbRepository : ITvdbRepository
    {
        public TvdbRepository(ITvdbHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            this.Handler = handler;
        }

        public IEnumerable<TvdbSearchResult> SearchSeries(string name)
        {
            var results = this.Handler.SearchSeries(name);

            return results;
        }

        public EpisodeData GetEpisodeInfo(int seriesId, int seasonNumber, int episodeNumber)
        {
            var episodeData = this.Handler.GetEpisode(seriesId, seasonNumber, episodeNumber);

            return episodeData;
        }

        #region Private Properties
        private ITvdbHandler Handler { get; set; }
        #endregion
    }
}
