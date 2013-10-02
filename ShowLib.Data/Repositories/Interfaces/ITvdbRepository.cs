using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowLib.Data.Entities;

namespace ShowLib.Data.Repositories
{
    public interface ITvdbRepository
    {
        Task<IEnumerable<TvdbSearchResult>> SearchSeries(string seriesName);

        Task<EpisodeData> GetEpisodeInfo(int seriesId, int seasonNumber, int episodeNumber);
    }
}
