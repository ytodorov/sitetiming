using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes
{
    public class QueryParams
    {
        public string? Where { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }

        public string? Order { get; set; }

        public string? Include { get; set; }

        public string? Select { get; set; }

    }
}
