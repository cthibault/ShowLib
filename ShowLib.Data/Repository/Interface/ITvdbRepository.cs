using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowLib.Data.Entity;

namespace ShowLib.Data.Repository
{
    public interface ITvdbRepository
    {
        IEnumerable<TvdbSearchResult> SearchSeries(string seriesName);

        EpisodeData GetEpisodeInfo(int seriesId, int seasonNumber, int episodeNumber);
    }
}
