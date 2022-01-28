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
    public class MyCollectionDataLoader<T> : DataLoaderBase<long, IEnumerable<T>> where T : BaseEntity
    {
        private readonly IServiceProvider _rootServiceProvider;
        public MyCollectionDataLoader(IServiceProvider serviceProvider) : base(false)
        {
            _rootServiceProvider = serviceProvider;
        }

        protected override async Task FetchAsync(IEnumerable<DataLoaderPair<long, IEnumerable<T>>> list, CancellationToken cancellationToken)
        {
            using (var scope = _rootServiceProvider.CreateScope())
            {
                SiteTimingContext dbContext = scope.ServiceProvider.GetRequiredService<SiteTimingContext>();

                List<long> ids = list.Select(pair => pair.Key).ToList();
                IEnumerable<T> data = await dbContext.Set<T>().Where(orderItem => ids.Contains(orderItem.Id)).ToListAsync(cancellationToken);
                ILookup<long, T> dataLookup = data.ToLookup(x => (long)x.Id);
                foreach (DataLoaderPair<long, IEnumerable<T>> entry in list)
                {
                    entry.SetResult(dataLookup[entry.Key]);
                }
            }
        }
    }
}
