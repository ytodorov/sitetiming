using System;
using System.Linq.Dynamic.Core;
using System.Text;
using Core.Classes;
using Core.Entities;
using Core.Extensions;
using Core.GraphQL.Types;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Mitsubishi.MCMachinery.Core.GraphQL.Types;
using PlaywrightTestLinuxContainer;

namespace Mitsubishi.MCMachinery.Core.GraphQL
{
    public class StarWarsQuery : ObjectGraphType<object>
    {
        public StarWarsQuery(StarWarsData data)
        {
            Name = "Query";

            Field<CharacterInterface>("hero", resolve: context => data.GetDroidByIdAsync("3"));
            Field<HumanType>(
                "human",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the human" }
                ),
                resolve: context => data.GetHumanByIdAsync(context.GetArgument<string>("id"))
            );

            Func<IResolveFieldContext, string, object> func = (context, id) => data.GetDroidByIdAsync(id);

            FieldDelegate<DroidType>(
                "droid",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the droid" }
                ),
                resolve: func
            );

            Func<IResolveFieldContext, string, object> func2 = (context, id) =>
            {
                using SiteTimingContext siteTimingContext = new SiteTimingContext();
                var queryParams = GetQueryParamsFromContext(context, typeof(SiteEntity));
                var entities = siteTimingContext.Sites
                .OrderBy(queryParams.Order)
                .Where(queryParams.Where)
                .Skip(queryParams.Skip.GetValueOrDefault())
                .Take(queryParams.Take.GetValueOrDefault())
                .Select(typeof(SiteEntity), queryParams.Select)                
                .ToDynamicList()
                .Cast<SiteEntity>()
                .ToList();

                return entities;

            };

            FieldDelegate<ListGraphType<SiteObjectGraphType>>(
                "sites",
                arguments: GetDefaultCollectionQueryArguments(),
                resolve: func2
            );

            Func<IResolveFieldContext, string, object> func3 = (context, id) =>
            {
                using SiteTimingContext siteTimingContext = new SiteTimingContext();
                var queryParams = GetQueryParamsFromContext(context, typeof(ProbeEntity));

                var query = siteTimingContext.Probes
                .Where(queryParams.Where)
                .Skip(queryParams.Skip.GetValueOrDefault())
                .Take(queryParams.Take.GetValueOrDefault())
                .Select(typeof(ProbeEntity), queryParams.Select)
                .OrderBy(queryParams.Order).ToQueryString();

                //var entities = siteTimingContext.Probes
                //.OrderBy(queryParams.Order)
                //.Where(queryParams.Where)
                //.Skip(queryParams.Skip.GetValueOrDefault())
                //.Take(queryParams.Take.GetValueOrDefault())
                //.Select(typeof(ProbeEntity), queryParams.Select)
                //.ToDynamicList()
                //.Cast<ProbeEntity>()
                //.ToList();

                var entities = siteTimingContext.Probes.AsQueryable().ApplyQueryParams(queryParams).ToList();

                return entities;

            };

            FieldDelegate<ListGraphType<ProbeObjectGraphType>>(
                "probes",
                arguments: GetDefaultCollectionQueryArguments(),
                resolve: func3
            );

        }

        public QueryArguments GetDefaultCollectionQueryArguments()
        {
            QueryArguments result = new QueryArguments(
                   new QueryArgument<IntGraphType> { Name = nameof(QueryParams.Take).ToLowerInvariant() },
                   new QueryArgument<IntGraphType> { Name = nameof(QueryParams.Skip).ToLowerInvariant() },
                   new QueryArgument<StringGraphType> { Name = nameof(QueryParams.Order).ToLowerInvariant() },
                   new QueryArgument<StringGraphType> { Name = nameof(QueryParams.Where).ToLowerInvariant() });

            return result;
        }

        public QueryParams GetQueryParamsFromContext(IResolveFieldContext context, Type entityType)
        {
            int? take = context.GetArgument<int?>(nameof(QueryParams.Take).ToLowerInvariant());
            int? skip = context.GetArgument<int?>(nameof(QueryParams.Skip).ToLowerInvariant());
            string order = context.GetArgument<string>(nameof(QueryParams.Order).ToLowerInvariant());
            string where = context.GetArgument<string>(nameof(QueryParams.Where).ToLowerInvariant());
            var select = ExtractSelectClause(context, entityType);

            if (!take.HasValue)
            {
                take = int.MaxValue;
            }
            if (!skip.HasValue)
            {
                skip = 0;
            }
            if (string.IsNullOrEmpty(order))
            {
                order = "Id desc";
            }
            if (string.IsNullOrEmpty(where))
            {
                where = "Id != -1";
            }


            //System.Type entityType = ((ListGraphType)context.FieldDefinition.ResolvedType).Type.BaseType.GetGenericArguments()[0];

            //if (string.IsNullOrEmpty(order))
            //{
            //    order = TypeUtility.GetSortingColumnForEntity(entityType);
            //}

            QueryParams querParams = new QueryParams()
            {
                Order = order,
                Skip = skip,
                Where = where,
                Take = take,
                Select = select,
            };

            return querParams;
        }

        public static string ExtractSelectClause(IResolveFieldContext context, Type entityType)
        {
            List<string> subfields = context.SubFields.Keys.ToList();

            // important for fixing client side evaluation:

            if (!subfields.Contains("id"))
            {
                subfields.Add("id");
            }

            List<string> keys = context.SubFields.Keys.ToList();

            foreach (string key in keys)
            {
                if (context.SubFields[key].Alias != null)
                {
                    subfields.Remove(context.SubFields[key].Alias);
                    subfields.Add(context.SubFields[key].Name);
                }
            }

            // No need for this. If it is in the list it will break our logic
            subfields.Remove("__typename");

            //Type entityType = null;
            //if (context.FieldDefinition.ResolvedType is ListGraphType listGraphType)
            //{
            //    entityType = listGraphType.Type.BaseType.GetGenericArguments()[0];
            //}
            //else
            //{
            //    GraphType graphType = context.FieldDefinition.ResolvedType as GraphType;

            //    entityType = graphType.GetType().BaseType.GetGenericArguments()[0];
            //}

            List<Type> simpleTypes = new List<Type>()
            {
                typeof(string),
                typeof(byte), typeof(byte?), typeof(short), typeof(short?), typeof(int), typeof(int?), typeof(long), typeof(long?),
                typeof(bool), typeof(bool?),
                typeof(Guid), typeof(Guid?),
                typeof(DateTime), typeof(DateTime?), typeof(DateTimeOffset), typeof(DateTimeOffset?),
                typeof(decimal), typeof(decimal?), typeof(float), typeof(float?), typeof(double), typeof(double?),
            };

         

            List<string> allProperiesOfEntity = entityType.GetProperties().Select(s => s.Name).ToList();

            List<string> simplePropNames = entityType.GetProperties().Where(p => simpleTypes.Contains(p.PropertyType))
                .Select(t => t.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append("new (");

            foreach (string field in subfields)
            {
                if (simplePropNames.Any(spn => spn.Equals(field, StringComparison.InvariantCultureIgnoreCase)))
                {
                    sb.Append($"{field[0].ToString().ToUpperInvariant()}{field.Substring(1)}");
                    if (subfields.IndexOf(field) != subfields.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
            }

            sb.Append(")");

            string select = sb.ToString();

            return select;
        }
    }
}
