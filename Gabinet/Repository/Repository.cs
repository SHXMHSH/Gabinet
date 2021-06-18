using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gabinet.Data;
using Gabinet.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Gabinet.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private GabinetDbContext _context;
        private DbSet<T> _dbset;
        public Repository(GabinetDbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public async Task Create(T model)
        {
            await _context.AddAsync<T>(model);
        }

        public T Delete(T model)
        {
            _context.Remove<T>(model);
            return model;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, object>> include = null, Expression<Func<T,bool>> where = null,Expression < Func<T, object> > includeTwo = null)
        {
            IQueryable<T> TList = null;
            if (where != null)
            {
                TList = _dbset.Where(where);
            }

            if (include != null && includeTwo != null)
            {
                TList = _dbset.Include(include).Include(includeTwo);
            }
            else
            {

                if (include != null)
                {
                    TList = _dbset.Include(include);
                }
                if (includeTwo != null)
                {
                    TList = _dbset.Include(includeTwo);
                }
            }

            return TList ?? _dbset;
        }
        public async Task<T> FindById(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public void Update(T model)
        {

            _context.Entry<T>(model).State = EntityState.Modified;
        }

        public Task<T> Select(Expression<Func<T, bool>> exp)
        {
            return _dbset.SingleAsync(exp);
        }
    }
}
