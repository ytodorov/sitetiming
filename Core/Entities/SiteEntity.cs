using Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class SiteEntity : BaseEntity
    {
        public string? Url { get; set; }
        public string? Name { get; set; }

        public string? Title { get; set; }

        public string? ScreenshotBase64 { get; set; }

        public string? FaviconBase64 { get; set; }

        public List<ProbeEntity>? Probes { get; set; }

        [NotMapped]
        public ProbeEntity? LastProbe { get; set; }

    }
}
