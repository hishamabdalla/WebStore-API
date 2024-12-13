using Store.Core.Entities;
using Store.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core
{
    /// <summary>
    /// Defines the Unit of Work pattern for managing repositories and committing changes.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits all changes made in the current unit of work to the database asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        Task<int> CompleteAsync();
        Task<int> CompleteIdentityAsync();

        /// <summary>
        /// Provides access to a generic repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity for the repository.</typeparam>
        /// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
        /// <returns>
        /// An instance of <see cref="IGenericRepository{TEntity, TKey}"/> for the specified entity type.
        /// </returns>
        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }

}
