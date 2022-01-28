using Core.Entities;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GraphQL.Types
{
    public class SiteObjectGraphType : BaseObjectGraphType<SiteEntity>
    {
        public SiteObjectGraphType()
        {
                //Field<ListGraphType<ProbeObjectGraphType>, IEnumerable<ProbeEntity>>()
                //   .Name(nameof(SiteEntity.Probes))
                //    .ResolveAsync(context =>
                //    {
                //        //return loader.LoadAsync(1).GetResultAsync();
                //        return loader.LoadAsync(context.Source.Id);
                //    });
        }
    }
}
