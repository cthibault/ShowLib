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

        public override async Task<IList<Show>> LoadAll()
        {
            var results = await base.LoadAll();

            return results.OrderBy(s => s.Title).ToList();
        }

        public async Task<Show> Load(Show entity)
        {
            if (entity != null)
            {
                entity = await this.Load(entity.Id);
            }

            return entity;
        }
    }
}
