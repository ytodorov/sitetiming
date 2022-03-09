using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ConsoleMessageEntity : BaseEntity
    {
        //
        // Summary:
        //     URL of the resource followed by 0-based line and column numbers in the resource
        //     formatted as URL:line:column.
        public string? Location { get; set; }

        //
        // Summary:
        //     The text of the console message.
        public string? Text { get; set; }

        //
        // Summary:
        //     One of the following values: 'log', 'debug', 'info', 'error', 'warning', 'dir',
        //     'dirxml', 'table', 'trace', 'clear', 'startGroup', 'startGroupCollapsed', 'endGroup',
        //     'assert', 'profile', 'profileEnd', 'count', 'timeEnd'.
        public string? Type { get; set; }

        [ForeignKey(nameof(Probe))]
        public int ProbeId { get; set; }

        public ProbeEntity? Probe { get; set; }
    }
}
