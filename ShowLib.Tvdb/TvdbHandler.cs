using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowLib.Data;
using ShowLib.Data.Entity;

namespace ShowLib.Tvdb
{
    public class TvdbHandler : ITvdbHandler
    {
        public TvdbHandler(string apiKey)
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException("apiKey");
            }

            this._apiKey = apiKey;
        }

        public IEnumerable<TvdbSearchResult> SearchSeries(string name)
        {
            var tvdbResults = this.Handler.SearchSeries(name, TvdbLib.Data.TvdbLanguage.DefaultLanguage);

            var results = tvdbResults.Select(r => new TvdbSearchResult(r.Id, r.ImdbId, r.SeriesName, r.Overview, r.FirstAired));

            return results;
        }

        public EpisodeData GetEpisode(int seriesId, int seasonNumber, int episodeNumber)
        {
            var episode = this.Handler.GetEpisode(seriesId, seasonNumber, episodeNumber, TvdbLib.Data.TvdbEpisode.EpisodeOrdering.DefaultOrder, TvdbLib.Data.TvdbLanguage.DefaultLanguage);

            var result = new EpisodeData(episode.Id, episode.ImdbId, episode.SeriesId, episode.SeasonId, episode.LastUpdated, episode.AbsoluteNumber, episode.SeasonNumber, episode.EpisodeNumber, episode.EpisodeName, episode.Overview);

            return result;
        }

        #region Private Properties
        private TvdbLib.TvdbHandler Handler
        {
            get
            {
                if (this._handler == null)
                {
                    this._handler = new TvdbLib.TvdbHandler(this._apiKey);
                }
                return this._handler;
            }
        }
        #endregion

        #region Private Fields
        private string _apiKey = string.Empty;
        private TvdbLib.TvdbHandler _handler;
        #endregion
    }
}
