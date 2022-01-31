using System;
using System.Linq.Dynamic.Core;
using System.Text;
using Core.Classes;
using Core.Entities;
using Core.Extensions;
using Core.GraphQL;
using Core.GraphQL.Types;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Mitsubishi.MCMachinery.Core.GraphQL.Types;
using PlaywrightTestLinuxContainer;

namespace Mitsubishi.MCMachinery.Core.GraphQL
{
    public class SiteTimingQuery : ObjectGraphType<object>
    {
        public SiteTimingQuery()
        {
            Name = "Query";

            Func<IResolveFieldContext, string, object> func2 = (context, id) =>
            {
                using SiteTimingContext siteTimingContext = new SiteTimingContext();
                var queryParams = GraphQlHelpers.GetQueryParamsFromContext(context, typeof(SiteEntity));

                var entities = siteTimingContext.Sites.AsQueryable().ApplyQueryParams(queryParams).ToList();

                return entities;

            };

            FieldDelegate<ListGraphType<SiteObjectGraphType>>(
                "sites",
                arguments: GraphQlHelpers.GetDefaultCollectionQueryArguments(),
                resolve: func2
            );

            Func<IResolveFieldContext, string, object> func3 = (context, id) =>
            {
                using SiteTimingContext siteTimingContext = new SiteTimingContext();
                var queryParams = GraphQlHelpers.GetQueryParamsFromContext(context, typeof(ProbeEntity));

                var entities = siteTimingContext.Probes.AsQueryable().ApplyQueryParams(queryParams).ToList();

                return entities;

            };

            FieldDelegate<ListGraphType<ProbeObjectGraphType>>(
                "probes",
                arguments: GraphQlHelpers.GetDefaultCollectionQueryArguments(),
                resolve: func3
            );

        }
    }
}
