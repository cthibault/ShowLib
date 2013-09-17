using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowLib.Data.Entity;

namespace ShowLib.Data
{
    public interface ITvdbHandler
    {
        IEnumerable<TvdbSearchResult> SearchSeries(string name);

        EpisodeData GetEpisode(int seriesId, int seasonNumber, int episodeNumber);
    }
}
