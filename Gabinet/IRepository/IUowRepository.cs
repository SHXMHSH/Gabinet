using System;
using System.Threading.Tasks;

namespace Gabinet.IRepository
{
    //GenericUnitOfWork
    public interface IUowRepository
    {
        public IRepository<T> Repository<T>() where T : class;
        public Task SaveChangesAsync();
        public void SaveChanges();
    }
}
