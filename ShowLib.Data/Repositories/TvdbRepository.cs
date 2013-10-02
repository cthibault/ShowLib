using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Client;
using Entities = ShowLib.Data.Entities;
using TvdbLib;
using TvdbLib.Data;
using ShowLib.Data.Entities;

namespace ShowLib.Data.Repositories
{
    public class TvdbRepository : ITvdbRepository
    {
        public TvdbRepository()
        {
        }

        public async Task<IEnumerable<Entities.TvdbSearchResult>> SearchSeries(string name)
        {
            IEnumerable<Entities.TvdbSearchResult> results = null;

            var tvdbResults = this.Handler.SearchSeries(name, TvdbLanguage.DefaultLanguage);

            if (tvdbResults != null)
            {
                results = tvdbResults.Select(r => new Entities.TvdbSearchResult(r.Id, r.ImdbId, r.SeriesName, r.Overview, r.FirstAired));
            }

            return results ?? new List<Entities.TvdbSearchResult>(0);
        }

        public async Task<EpisodeData> GetEpisodeInfo(int seriesId, int seasonNumber, int episodeNumber)
        {
            EpisodeData result = null;

            var tvdbEpisode = this.Handler.GetEpisode(seriesId, seasonNumber, episodeNumber, TvdbEpisode.EpisodeOrdering.DefaultOrder, TvdbLanguage.DefaultLanguage);

            if (tvdbEpisode != null)
            {
                result = new EpisodeData(tvdbEpisode.Id,
                                         tvdbEpisode.ImdbId,
                                         tvdbEpisode.SeriesId,
                                         tvdbEpisode.SeasonId,
                                         tvdbEpisode.LastUpdated,
                                         tvdbEpisode.AbsoluteNumber,
                                         tvdbEpisode.SeasonNumber,
                                         tvdbEpisode.EpisodeNumber,
                                         tvdbEpisode.EpisodeName,
                                         tvdbEpisode.Overview);
            }

            return result;
        }

        #region Private Properties
        private TvdbHandler Handler
        {
            get
            {
                if (this._handler == null)
                {
                    this._handler = new TvdbHandler(Constants.Tvdb.API_KEY);
                }
                return this._handler;
            }
        }
        #endregion

        #region Private Fields
        private TvdbHandler _handler;
        #endregion
    }
}
