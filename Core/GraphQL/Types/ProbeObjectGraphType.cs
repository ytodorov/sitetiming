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
    public class ProbeObjectGraphType : BaseObjectGraphType<ProbeEntity>
    {
        public ProbeObjectGraphType(IDataLoaderContextAccessor accessor)
        {
            Field<SiteObjectGraphType, SiteEntity>()
               .Name(nameof(ProbeEntity.Site))
                .ResolveAsync(context =>
                {
                    var loaderCon = context.RequestServices.GetRequiredService<IDataLoaderContextAccessor>();

                    IDataLoader<int, SiteEntity> loader =
                        loaderCon.Context.GetOrAddBatchLoader<int, SiteEntity>(
                            "SiteGetByIdsAsync",
                            GetByIdsAsync);

                    return loader.LoadAsync(context.Source.SiteId);
                });
        }

        public async Task<IDictionary<int, SiteEntity>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            using var siteTimingContext = new SiteTimingContext();
            List<SiteEntity> entities = await siteTimingContext.Set<SiteEntity>().Where(t => ids.Contains(t.Id)).ToListAsync();

            Dictionary<int, SiteEntity> result = entities.ToDictionary(t => t.Id);

            return result;
        }
    }
}
