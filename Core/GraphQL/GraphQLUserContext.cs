using System.Collections.Generic;
using System.Security.Claims;

namespace Mitsubishi.MCMachinery.Core.GraphQL
{
    public class GraphQLUserContext : Dictionary<string, object>
    {
        public ClaimsPrincipal? User { get; set; }
    }
}
