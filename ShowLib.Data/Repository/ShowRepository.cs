using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowLib.Data.Entity;

namespace ShowLib.Data.Repository
{
    public class ShowRepository : IShowRepository
    {
        public async Task<IEnumerable<Show>> GetShows()
        {
            throw new NotImplementedException();
        }

        public Task<Show> GetShowDetail(int showId)
        {
            throw new NotImplementedException();
        }

        public Task<Show> GetShowDetail(Show show)
        {
            throw new NotImplementedException();
        }
    }
}
