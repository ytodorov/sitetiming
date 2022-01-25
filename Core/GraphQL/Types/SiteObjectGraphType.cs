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
            
        }
    }
}
