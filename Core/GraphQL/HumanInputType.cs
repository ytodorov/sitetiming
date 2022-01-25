using GraphQL.Types;
using Mitsubishi.MCMachinery.Core.GraphQL.Types;

namespace Mitsubishi.MCMachinery.Core.GraphQL
{
    public class HumanInputType : InputObjectGraphType<Human>
    {
        public HumanInputType()
        {
            Name = "HumanInput";
            Field(x => x.Name);
            Field(x => x.HomePlanet, nullable: true);
        }
    }
}
