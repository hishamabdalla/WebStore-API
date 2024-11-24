using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.Core.Entities;
using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    /// <summary>
    /// A utility class for evaluating and applying specifications to an entity query.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
    public static class SpecificationsEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        /// <summary>
        /// Applies the given specifications to the input query and returns the resulting query.
        /// </summary>
        /// <param name="inputQuery">The base query to which specifications will be applied.</param>
        /// <param name="spec">The specifications containing criteria and include expressions.</param>
        /// <returns>
        /// A queryable object that represents the input query with the specifications applied.
        /// </returns>
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec)
        {
            // Initialize the query with the input query.
            var query = inputQuery;

            // Apply the criteria (filtering) from the specifications if defined.
            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply include expressions to add related entities to the query.
            // Aggregate loops through each IncludeExpression and applies it to the current query.
            query = spec.Include.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            // Return the final query with all specifications applied.
            return query;
        }
    }

}
