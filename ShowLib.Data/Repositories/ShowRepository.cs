using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Client;
using ShowLib.Data.Entities;

namespace ShowLib.Data.Repositories
{
    class ShowRepository : BaseRepository<Show>, IShowRepository
    {
        internal ShowRepository(DocumentStore documentStore) : base(documentStore) { }

        public override async Task<KeyValuePair<bool, IList<Show>>> Save(IEnumerable<Show> entities)
        {
            foreach (var entity in entities.Where(s => s.Id < 0))
            {
                entity.Id = 0;
            }

            return await base.Save(entities);
        }
    }
}
