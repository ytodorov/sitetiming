using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RequestTimingResultEntity : BaseEntity
    {
        //
        // Summary:
        //     Request start time in milliseconds elapsed since January 1, 1970 00:00:00 UTC
        public float StartTime
        {
            get;
            set;
        }

        //
        // Summary:
        //     Time immediately before the browser starts the domain name lookup for the resource.
        //     The value is given in milliseconds relative to startTime, -1 if not available.
        public float DomainLookupStart
        {
            get;
            set;
        }

        //
        // Summary:
        //     Time immediately after the browser starts the domain name lookup for the resource.
        //     The value is given in milliseconds relative to startTime, -1 if not available.
        public float DomainLookupEnd
        {
            get;
            set;
        }

        //
        // Summary:
        //     Time immediately before the user agent starts establishing the connection to
        //     the server to retrieve the resource. The value is given in milliseconds relative
        //     to startTime, -1 if not available.
        public float ConnectStart
        {
            get;
            set;
        }

        //
        // Summary:
        //     Time immediately before the browser starts the handshake process to secure the
        //     current connection. The value is given in milliseconds relative to startTime,
        //     -1 if not available.
        public float SecureConnectionStart
        {
            get;
            set;
        }

        //
        // Summary:
        //     Time immediately before the user agent starts establishing the connection to
        //     the server to retrieve the resource. The value is given in milliseconds relative
        //     to startTime, -1 if not available.
        public float ConnectEnd
        {
            get;
            set;
        }

        //
        // Summary:
        //     Time immediately before the browser starts requesting the resource from the server,
        //     cache, or local resource. The value is given in milliseconds relative to startTime,
        //     -1 if not available.
        public float RequestStart
        {
            get;
            set;
        }

        //
        // Summary:
        //     Time immediately after the browser starts requesting the resource from the server,
        //     cache, or local resource. The value is given in milliseconds relative to startTime,
        //     -1 if not available.
        public float ResponseStart
        {
            get;
            set;
        }

        //
        // Summary:
        //     Time immediately after the browser receives the last byte of the resource or
        //     immediately before the transport connection is closed, whichever comes first.
        //     The value is given in milliseconds relative to startTime, -1 if not available.
        public float ResponseEnd
        {
            get;
            set;
        }
    }
}
