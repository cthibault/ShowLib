using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowLib.Data.Entities;

namespace ShowLib.Data.Repositories
{
    public class MocksTvdbRepository : ITvdbRepository
    {
        public async Task<IEnumerable<TvdbSearchResult>> SearchSeries(string seriesName)
        {
            return new List<TvdbSearchResult>();
        }

        public async Task<EpisodeData> GetEpisodeInfo(int seriesId, int seasonNumber, int episodeNumber)
        {
            return new EpisodeData(0, string.Empty, 0, 0, DateTime.Now, 0, 0, 0, string.Empty, string.Empty);
        }
    }
}
