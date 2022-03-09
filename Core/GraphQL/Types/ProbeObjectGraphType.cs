using Core.Entities;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlaywrightTestLinuxContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GraphQL.Types
{
    public class ProbeObjectGraphType : BaseObjectGraphType<ProbeEntity>
    {
        public ProbeObjectGraphType(MyEntityDataLoader<SiteEntity> loader)
        {
            var f = Field<SiteObjectGraphType, SiteEntity>()
               .Name(nameof(ProbeEntity.Site))
                .ResolveAsync(context =>
                {
                    // Трябва задължително да се включва SiteId в полетата които се искат от UI
                    return loader.LoadAsync(context.Source.SiteId);
                });

            f.Argument<IntGraphType>("test");


            /*
              new QueryArgument<IntGraphType> { Name = nameof(QueryParams.Take).ToLowerInvariant() },
                   new QueryArgument<IntGraphType> { Name = nameof(QueryParams.Skip).ToLowerInvariant() },
                   new QueryArgument<StringGraphType> { Name = nameof(QueryParams.Order).ToLowerInvariant() },
                   new QueryArgument<StringGraphType> { Name = nameof(QueryParams.Where).ToLowerInvariant() }
             */
        }
    }
}
