using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLib.Data.Repositories
{
    public interface IRepository<T>
        where T : class
    {
        Task<IList<T>> LoadAll();

        Task<T> Load(int id);
        Task<T> Load(ValueType id);
        Task<IList<T>> Load(IEnumerable<int> ids);
        Task<IList<T>> Load(IEnumerable<ValueType> ids);

        Task<KeyValuePair<bool, IList<T>>> Save(T entity);
        Task<KeyValuePair<bool, IList<T>>> Save(IEnumerable<T> entities);

        Task<bool> Delete(int id);
        Task<bool> Delete(ValueType id);
        Task<bool> Delete(IEnumerable<int> ids);
        Task<bool> Delete(IEnumerable<ValueType> ids);
    }
}
