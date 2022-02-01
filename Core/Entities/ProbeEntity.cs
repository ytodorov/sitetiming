﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{  

    public class ProbeEntity : ProbeTimingResultEntity
    {
        public string? SourceIpAddress { get; set; }

        public string? DestinationIpAddress { get; set; }

        public bool? IsSuccessfull { get; set; }

        public string? ExceptionMessage { get; set; }

        public string? ExceptionStackTrace { get; set; }

        [ForeignKey(nameof(Site))]
        public int SiteId { get; set; }
        public SiteEntity? Site { get; set; }

        public List<RequestEntity>? Requests { get; set; }

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
