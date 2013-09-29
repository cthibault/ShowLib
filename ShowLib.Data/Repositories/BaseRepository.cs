using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Client;

namespace ShowLib.Data.Repositories
{
    internal abstract class BaseRepository<T> : IRepository<T>
        where T : class
    {
        internal BaseRepository(DocumentStore documentStore)
        {
            this.DocumentStore = documentStore;
        }

        protected DocumentStore DocumentStore { get; set; }

        public virtual async Task<IList<T>> LoadAll()
        {
            IList<T> results = null;

            using (var session = this.DocumentStore.OpenAsyncSession())
            {
                try
                {
                    results = await session.Query<T>().ToListAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }

            return results ?? new List<T>();
        }

        public virtual async Task<T> Load(int id)
        {
            return await this.Load((ValueType)id);
        }
        public virtual async Task<T> Load(ValueType id)
        {
            T result = null;

            using (var session = this.DocumentStore.OpenAsyncSession())
            {
                try
                {
                    result = await session.LoadAsync<T>(id);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }

            return result;
        }
        public virtual async Task<IList<T>> Load(IEnumerable<int> ids)
        {
            return await this.Load(ids.Cast<ValueType>());
        }
        public virtual async Task<IList<T>> Load(IEnumerable<ValueType> ids)
        {
            IList<T> results = null;

            using (var session = this.DocumentStore.OpenAsyncSession())
            {
                try
                {
                    results = await session.LoadAsync<T>(ids);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }

            return results ?? new List<T>();
        }


        public virtual async Task<KeyValuePair<bool, IList<T>>> Save(T entity)
        {
            return await this.Save(new List<T> { entity });
        }
        public virtual async Task<KeyValuePair<bool, IList<T>>> Save(IEnumerable<T> entities)
        {
            bool success = false;

            if (entities != null && entities.Any())
            {
                using (var session = this.DocumentStore.OpenAsyncSession())
                {
                    try
                    {
                        foreach (var entity in entities)
                        {
                            await session.StoreAsync(entity);
                        }

                        await session.SaveChangesAsync();

                        success = true;
                    }
                    catch (Exception ex)
                    {
                        //TODO: Exceptiong Logging
                        success = false;
                        System.Diagnostics.Debugger.Break();
                    }
                }
            }

            return new KeyValuePair<bool,IList<T>>(success, entities.ToList());
        }


        public virtual async Task<bool> Delete(int id)
        {
            return await this.Delete(new List<ValueType> { (ValueType)id });
        }
        public virtual async Task<bool> Delete(ValueType id)
        {
            return await this.Delete(new List<ValueType> { id });
        }
        public virtual async Task<bool> Delete(IEnumerable<int> ids)
        {
            bool success = false;

            if (ids != null)
            {
                success = await this.Delete(ids.Cast<ValueType>());
            }

            return success;
        }
        public virtual async Task<bool> Delete(IEnumerable<ValueType> ids)
        {
            bool success = false;

            if (ids != null && ids.Any())
            {
                using (var session = this.DocumentStore.OpenAsyncSession())
                {
                    try
                    {
                        var entities = await session.LoadAsync<T>(ids);

                        foreach (var entity in entities)
                        {
                            session.Delete<T>(entity);
                        }

                        await session.SaveChangesAsync();

                        success = true;
                    }
                    catch (Exception ex)
                    {
                        //TODO: Exceptiong Logging
                        success = false;
                        System.Diagnostics.Debugger.Break();
                    }
                }
            }

            return success;
        }
    }
}
