using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines a specification pattern for querying entities with filtering and inclusion of related data.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the criteria for filtering entities.
        /// This is a LINQ expression defining a predicate to filter the data.
        /// </summary>
        Expression<Func<TEntity, bool>> Criteria { get; set; }

        /// <summary>
        /// Gets or sets the list of include expressions to include related entities in the query.
        /// Each expression specifies a navigation property to include.
        /// </summary>
        List<Expression<Func<TEntity, object>>> Include { get; set; }
    }

}
