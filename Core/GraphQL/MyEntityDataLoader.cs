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

    public class MyEntityDataLoader<T> : DataLoaderBase<int, T> where T : BaseEntity
    {
        private readonly IServiceProvider _rootServiceProvider;
        public MyEntityDataLoader(IServiceProvider serviceProvider) : base(false)
        {
            _rootServiceProvider = serviceProvider;
        }

        protected override async Task FetchAsync(IEnumerable<DataLoaderPair<int, T>> list, CancellationToken cancellationToken)
        {
            using (var scope = _rootServiceProvider.CreateScope())
            {
                SiteTimingContext dbContext = scope.ServiceProvider.GetRequiredService<SiteTimingContext>();

                IEnumerable<int> ids = list.Select(pair => pair.Key);
                IDictionary<int, T> data = await dbContext.Set<T>().Where(entity => ids.Contains(entity.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
                foreach (DataLoaderPair<int, T> entry in list)
                {
                    entry.SetResult(data.TryGetValue(entry.Key, out var order) ? order : null);
                }
            }
        }
    }

    


}

/*
// similar to CollectionBatchDataLoader
public class MyOrderItemsDataLoader : DataLoaderBase<int, IEnumerable<OrderItem>>
{
    private readonly MyDbContext _dbContext;
    public MyOrderItemsDataLoader(MyDbContext dataContext)
    {
        _dbContext = dataContext;
    }

    protected override Task FetchAsync(IEnumerable<DataLoaderPair<int, IEnumerable<OrderItem>>> list, CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IEnumerable<OrderItem> data = await _dbContext.OrderItems.Where(orderItem => ids.Contains(orderItem.OrderId)).ToListAsync(cancellationToken);
        ILookup<int, OrderItem> dataLookup = data.ToLookup(x => x.OrderId);
        foreach (DataLoaderPair<int, IEnumerable<OrderItem>> entry in list)
        {
            entry.SetResult(dataLookup[entry.Key]);
        }
    }
}
*/