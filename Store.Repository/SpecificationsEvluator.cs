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
    public static class SpecificationsEvluator<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        //create and Return Query
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity,TKey> spec)
        {
            var query = inputQuery;

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

           query= spec.Include.Aggregate(query,(currentQuery,IncludeExpression) => currentQuery.Include(IncludeExpression));

            return query;
        }
    }
}
