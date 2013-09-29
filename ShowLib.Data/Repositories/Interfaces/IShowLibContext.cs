using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowLib.Data.Entities;

namespace ShowLib.Data.Repositories
{
    public interface IShowLibContext
    {
        string Url { get; }
        IShowRepository ShowRepository { get; }
    }
}
