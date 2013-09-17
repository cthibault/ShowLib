using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowLib.Data.Entity;

namespace ShowLib.Data.Repository
{
    public class MocksTvdbRepository : ITvdbRepository
    {
        public IEnumerable<TvdbSearchResult> SearchSeries(string name)
        {
            throw new NotImplementedException();
        }

        public EpisodeData GetEpisodeInfo(int seriesId, int seasonNumber, int episodeNumber)
        {
            throw new NotImplementedException();
        }
    }
}
