using Core.Classes;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GraphQL
{
    public static class GraphQlHelpers
    {

        public static QueryArguments GetDefaultCollectionQueryArguments()
        {
            QueryArguments result = new QueryArguments(
                   new QueryArgument<IntGraphType> { Name = nameof(QueryParams.Take).ToLowerInvariant() },
                   new QueryArgument<IntGraphType> { Name = nameof(QueryParams.Skip).ToLowerInvariant() },
                   new QueryArgument<StringGraphType> { Name = nameof(QueryParams.Order).ToLowerInvariant() },
                   new QueryArgument<StringGraphType> { Name = nameof(QueryParams.Where).ToLowerInvariant() });

            return result;
        }

        public static QueryParams GetQueryParamsFromContext(IResolveFieldContext context, Type entityType)
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
