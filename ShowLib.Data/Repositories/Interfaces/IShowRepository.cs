using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowLib.Data.Entities;

namespace ShowLib.Data.Repositories
{
    public interface IShowRepository : IRepository<Show>
    {
        Task<Show> Load(Show entity);
    }
}
