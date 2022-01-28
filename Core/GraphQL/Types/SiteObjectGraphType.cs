using Core.Entities;
using GraphQL.Types;
using GraphQL.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaywrightTestLinuxContainer;

namespace Core.GraphQL.Types
{
    public class SiteObjectGraphType : BaseObjectGraphType<SiteEntity>
    {
        public SiteObjectGraphType()
        {
           
        }
    }
}
