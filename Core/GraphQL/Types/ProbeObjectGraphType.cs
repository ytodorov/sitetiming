﻿using Core.Entities;
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
    public class ProbeObjectGraphType : BaseObjectGraphType<ProbeEntity>
    {
        public ProbeObjectGraphType(MyEntityDataLoader<SiteEntity> loader)
        {
            Field<SiteObjectGraphType, SiteEntity>()
               .Name(nameof(ProbeEntity.Site))
                .ResolveAsync(context =>
                {
                    return loader.LoadAsync(context.Source.SiteId);
                });
        }
    }
}
