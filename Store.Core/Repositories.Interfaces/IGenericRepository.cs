using Store.Core.Entities;
using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repositories.Interfaces
{
    /// <summary>
    /// Defines a generic repository for performing CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being managed.</typeparam>
    /// <typeparam name="TKey">The type of the entity's unique identifier.</typeparam>
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        /// <summary>
        /// Retrieves all entities that satisfy the given specifications.
        /// </summary>
        /// <param name="spec">The specifications used to filter and include related data.</param>
        /// <returns>
        /// A task representing the operation, returning a collection of entities that match the criteria.
        /// </returns>
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec);

        /// <summary>
        /// Retrieves a single entity that matches the given specifications.
        /// </summary>
        /// <param name="spec">The specifications used to filter and include related data.</param>
        /// <returns>
        /// A task representing the operation, returning the matching entity, or null if no match is found.
        /// </returns>
        Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec);


        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The entity with the given identifier, or null if not found.</returns>
        Task<TEntity> GetAsync(TKey id);

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        Task AddAsync(TEntity entity);

        Task<int> GetCountAsync(ISpecifications<TEntity,TKey> spec);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Delete(TEntity entity);


    }

}
