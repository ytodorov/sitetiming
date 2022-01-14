using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RequestEntity : RequestTimingResultEntity
    {
        //
        // Summary:
        //     The method returns null unless this request has failed, as reported by requestfailed
        //     event.
        //     Example of logging of all the failed requests:
        //     page.RequestFailed += (_, request) =>
        //     {
        //     Console.WriteLine(request.Failure);
        //     };
        public string? Failure { get; set; }

        //
        // Summary:
        //     Whether this request is driving frame's navigation.
        public bool IsNavigationRequest { get; set; }

        //
        // Summary:
        //     Request's method (GET, POST, etc.)
        public string Method { get; set; }

        //
        // Summary:
        //     Request's post body, if any.
        public string? PostData { get; set; }

        //
        // Summary:
        //     Contains the request's resource type as it was perceived by the rendering engine.
        //     ResourceType will be one of the following: document, stylesheet, image, media,
        //     font, script, texttrack, xhr, fetch, eventsource, websocket, manifest, other.
        public string ResourceType { get; set; }

        //
        // Summary:
        //     Returns resource timing information for given request. Most of the timing values
        //     become available upon the response, responseEnd becomes available when request
        //     finishes. Find more information at Resource Timing API.
        //     var request = await page.RunAndWaitForRequestFinishedAsync(async () =>
        //     {
        //     await page.GotoAsync("https://www.microsoft.com");
        //     });
        //     Console.WriteLine(request.Timing.ResponseEnd);

        //
        // Summary:
        //     URL of the request.
        public string Url { get; set; }

        [ForeignKey(nameof(Probe))]
        public int ProbeId { get; set; }

        public ProbeEntity? Probe { get; set; }
    }
}
