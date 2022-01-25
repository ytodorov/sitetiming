using Core.Entities;
using GraphQL.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GraphQL.Types
{
    public class ProbeObjectGraphType : BaseObjectGraphType<ProbeEntity>
    {
        public ProbeObjectGraphType()
        {
            //Field<SiteObjectGraphType, SiteEntity>()
            //   .Name(nameof(ProbeEntity.Site))
            //    .ResolveAsync(context =>
            //    {
            //        IDataLoader<int, SiteEntity> loader =
            //            accessor.Context.GetOrAddBatchLoader<int, SiteEntity>(
            //                nameof(ISimulationRepository) + nameof(ISimulationRepository.GetByIdsAsync),
            //                simulationRepository.GetByIdsAsync);

            //        return loader.LoadAsync(context.Source.SimulationId.GetValueOrDefault());
            //    });
        }
    }
}
