using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mitsubishi.MCMachinery.Core.GraphQL
{
    public class SiteTimingSchema : Schema
    {
        public SiteTimingSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = provider.GetRequiredService<SiteTimingQuery>();
            //Mutation = provider.GetRequiredService<StarWarsMutation>();
        }
    }
}
