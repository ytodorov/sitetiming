using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{  

    public class ProbeEntity : ProbeTimingResultEntity
    {
        public string? SourceIpAddress { get; set; }

        public string? SourceIpAddressHostname { get; set; }

        public string? SourceIpAddressCity { get; set; }

        public string? SourceIpAddressRegion { get; set; }

        public string? SourceIpAddressCountry { get; set; }


        public string? SourceIpAddressPostal { get; set; }

        public string? SourceIpAddressTimezone { get; set; }

        public string? SourceIpAddressOrg { get; set; }

        public double? SourceIpAddressLatitude { get; set; }

        public double? SourceIpAddressLongitude { get; set; }

        public string? DestinationIpAddress { get; set; }

        public string? DestinationIpAddressHostname { get; set; }

        public string? DestinationIpAddressCity { get; set; }

        public string? DestinationIpAddressRegion { get; set; }

        public string? DestinationIpAddressCountry { get; set; }

        public string? DestinationIpAddressPostal { get; set; }

        public string? DestinationIpAddressTimezone { get; set; }

        public string? DestinationIpAddressOrg { get; set; }

        public double? DestinationIpAddressLatitude { get; set; }

        public double? DestinationIpAddressLongitude { get; set; }

        public double? DistanceBetweenIpAddresses { get; set; }

        public bool? IsSuccessfull { get; set; }

        public string? ExceptionMessage { get; set; }

        public string? ExceptionStackTrace { get; set; }

        [ForeignKey(nameof(Site))]
        public int SiteId { get; set; }
        public SiteEntity? Site { get; set; }

        public List<RequestEntity>? Requests { get; set; }

        public List<ConsoleMessageEntity>? ConsoleMessages { get; set; }

        public long? TimetakenToGenerateInMs { get; set; }

        public string? ScreenshotUrl { get; set; }

        public string? ScreenshotBase64 { get; set; }

        public string? VideoUrl { get; set; }

        public override string ToString()
        {
            return $"Url: {Site?.Url}{Environment.NewLine}TimetakenToGenerateInMs:{TimetakenToGenerateInMs}{Environment.NewLine}Latency: {LatencyInChrome}{Environment.NewLine}DomLoaded: {DOMContentLoadedEventInChrome}{Environment.NewLine}";
        }
    }
}
