using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gabinet.Data;
using Gabinet.IRepository;

namespace Gabinet.Repository
{
    public class UowRepository : IUowRepository
    {
       private readonly GabinetDbContext db;

        public UowRepository(GabinetDbContext dbContext) => db = dbContext;

        public Dictionary<Type, object> repositories = new();
        public IRepository<T> Repository<T>() where T : class
        {
            if(repositories.Keys.Contains(typeof(T))){
                return repositories[typeof(T)] as IRepository<T>;
            }
            IRepository<T> repo = new Repository<T>(db);
            repositories.Add(typeof(T), repo);
            return repo;
            
            
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }


    }
}
