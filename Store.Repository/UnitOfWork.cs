using Store.Core;
using Store.Core.Entities;
using Store.Core.Repositories.Interfaces;
using Store.Repository.Data.Contexts;
using Store.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            if (!_repositories.ContainsKey(typeof(TEntity).Name))
            {
                var repository = new GenericRepository<TEntity, TKey>(_context);
                _repositories.Add(typeof(TEntity).Name, repository);
            }

            return _repositories[typeof(TEntity).Name] as IGenericRepository<TEntity, TKey>;
        }
    }
}
