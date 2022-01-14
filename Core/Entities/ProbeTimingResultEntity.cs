using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProbeTimingResultEntity : BaseEntity
    {
        public long ConnectEnd { get; set; }
        public long ConnectStart { get; set; }
        public long DomComplete { get; set; }
        public long DomContentLoadedEventEnd { get; set; }
        public long DomContentLoadedEventStart { get; set; }
        public long DomInteractive { get; set; }
        public long DomLoading { get; set; }
        public long DomainLookupEnd { get; set; }
        public long DomainLookupStart { get; set; }
        public long FetchStart { get; set; }
        public long LoadEventEnd { get; set; }
        public long LoadEventStart { get; set; }
        public long NavigationStart { get; set; }
        public long RedirectEnd { get; set; }
        public long RedirectStart { get; set; }
        public long RequestStart { get; set; }
        public long ResponseEnd { get; set; }
        public long ResponseStart { get; set; }
        public long SecureConnectionStart { get; set; }
        public long UnloadEventEnd { get; set; }
        public long UnloadEventStart { get; set; }

        public long DOMContentLoadedEventInChrome { get; set; }

        public long LatencyInChrome { get; set; }

        public long LoadEventInChrome { get; set; }
    }
}

/*
    connectEnd: 1641398285104
connectStart: 1641398285104
domComplete: 1641398289707
domContentLoadedEventEnd: 1641398287914
domContentLoadedEventStart: 1641398287820
domInteractive: 1641398287819
domLoading: 1641398285866
domainLookupEnd: 1641398285104
domainLookupStart: 1641398285104
fetchStart: 1641398285104
loadEventEnd: 1641398289711
loadEventStart: 1641398289707
navigationStart: 1641398285099
redirectEnd: 0
redirectStart: 0
requestStart: 1641398285116
responseEnd: 1641398285935
responseStart: 1641398285857
secureConnectionStart: 0
unloadEventEnd: 0
unloadEventStart: 0
   https://www.html5rocks.com/en/tutorials/webperformance/basics/
   Data from the API really comes to life when events are used in combination:

Network latency (): responseEnd-fetchStart
The time taken for page load once the page is received from the server: loadEventEnd-responseEnd
The whole process of navigation and page load: loadEventEnd-navigationStart.
   */