using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowLib.Data.Entity;

namespace ShowLib.Data.Repository
{
    public interface IShowRepository
    {
        Task<IEnumerable<Show>> GetShows();

        Task<Show> GetShowDetail(int showId);

        Task<Show> GetShowDetail(Show show);
    }
}
