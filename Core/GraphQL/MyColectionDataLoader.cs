using Core.Entities;
using GraphQL;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlaywrightTestLinuxContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GraphQL
{
    public class MyCollectionDataLoader<T> : DataLoaderBase<int, IEnumerable<T>> where T : BaseEntity
    {
        private readonly IServiceProvider _rootServiceProvider;
        public MyCollectionDataLoader(IServiceProvider serviceProvider) : base(false)
        {
            _rootServiceProvider = serviceProvider;
        }

        protected override async Task FetchAsync(IEnumerable<DataLoaderPair<int, IEnumerable<T>>> list, CancellationToken cancellationToken)
        {
            using (var scope = _rootServiceProvider.CreateScope())
            {
                SiteTimingContext dbContext = scope.ServiceProvider.GetRequiredService<SiteTimingContext>();

                IResolveFieldContext resolveFieldContext = scope.ServiceProvider.GetRequiredService<IResolveFieldContext>();

                List<int> ids = list.Select(pair => pair.Key).ToList();
                IEnumerable<T> data = await dbContext.Set<T>().Where(orderItem => ids.Contains(orderItem.Id)).ToListAsync(cancellationToken);
                ILookup<int, T> dataLookup = data.ToLookup(x => x.Id);
                foreach (DataLoaderPair<int, IEnumerable<T>> entry in list)
                {
                    entry.SetResult(dataLookup[entry.Key]);
                }
            }
        }
    }
}
