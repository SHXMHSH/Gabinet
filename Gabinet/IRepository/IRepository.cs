using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gabinet.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, object>> include = null, Expression<Func<T, bool>> where = null, Expression<Func<T, object>> includeTwo = null);
        Task<T> Select(Expression<Func<T, bool>> exp);
        Task<T> FindById(int id);
        Task Create(T model);
        void Update(T model);
        T Delete(T model);
    }
}
