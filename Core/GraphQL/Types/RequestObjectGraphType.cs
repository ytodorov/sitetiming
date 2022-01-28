using Core.Entities;
using GraphQL.DataLoader;
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
    public class RequestObjectGraphType : BaseObjectGraphType<RequestEntity>
    {
        public RequestObjectGraphType(MyEntityDataLoader<ProbeEntity> loader)
        {
            Field<ProbeObjectGraphType, ProbeEntity>()
               .Name(nameof(RequestEntity.Probe))
                .ResolveAsync(context =>
                {
                    return loader.LoadAsync(context.Source.ProbeId);
                });
        }
    }
}
