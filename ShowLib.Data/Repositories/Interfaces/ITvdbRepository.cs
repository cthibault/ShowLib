using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowLib.Data.Entities;

namespace ShowLib.Data.Repositories
{
    public interface ITvdbRepository
    {
        IEnumerable<TvdbSearchResult> SearchSeries(string seriesName);

        EpisodeData GetEpisodeInfo(int seriesId, int seasonNumber, int episodeNumber);
    }
}
