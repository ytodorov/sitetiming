using Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyQueryParams<T>(this IQueryable<T> query, QueryParams queryParams)
        {
            query = query
                .Where(queryParams.Where)
                .Skip(queryParams.Skip.GetValueOrDefault())
                .Take(queryParams.Take.GetValueOrDefault())
                .Select(typeof(T), queryParams.Select)
                .OrderBy(queryParams.Order)
                .Cast<T>()
                .AsQueryable();
                

            return query;
        }
    }
}
