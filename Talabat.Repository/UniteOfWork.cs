using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UniteOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;//make it hash table because in every time it need deffirent repo from change type but the dictionary must all same type
        public UniteOfWork(StoreContext context)
        {
            _context = context;
        }
        public async Task<int> Complete()
        {
         return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
           if(_repositories==null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if(!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_context);
                _repositories.Add(type, repository);

            }
            return (IGenericRepository<TEntity>) _repositories[type];

        }
    }
}
