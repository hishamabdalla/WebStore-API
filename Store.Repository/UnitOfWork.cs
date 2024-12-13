using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        // The database context used to interact with the data source.
        private readonly StoreDbContext _context;
        private readonly IdentityDbContext _identityContext;

        // A Hashtable to store instantiated repositories by entity type.
        private Hashtable _repositories;

 
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new Hashtable(); // Initializes the repository container.
        }

        
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public async Task<int> CompleteIdentityAsync() => await _identityContext.SaveChangesAsync();


        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            // Check if a repository for the given entity type already exists.
            if (!_repositories.ContainsKey(typeof(TEntity).Name))
            {
                // Create a new repository instance for the entity type.
                var repository = new GenericRepository<TEntity, TKey>(_context);

                // Store the repository instance in the Hashtable using the entity type's name as the key.
                _repositories.Add(typeof(TEntity).Name, repository);
            }

            // Retrieve and return the repository from the Hashtable.
            return _repositories[typeof(TEntity).Name] as IGenericRepository<TEntity, TKey>;
        }
    }
}

