using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Repository.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {

            var query = inputQuery;//_context.set<product>()
            if (spec.Criteria != null)//p=>p.id==10
                query = query.Where(spec.Criteria);
            //_context.set<product>().where(p=>p.id==10)
            if (spec.IsPaginationEnable)
                query = query.Skip(spec.Skip).Take(spec.Take);
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            //_context.set<product>().where(p=>p.id==10).include(p=>p.productbrand)
            //_context.set<product>().where(p=>p.id==10).include(p=>p.productbrand).include(p=>p.producttype)


            return query;
        }
    }
}
