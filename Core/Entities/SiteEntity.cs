namespace PlaywrightTestLinuxContainer.Entities
{
    public class SiteEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Title { get; set; }

        public string? ScreenshotBase64 { get; set; }

        public string? FaviconBase64 { get; set; }

        public int? Size { get; set; }

        public List<TimingEntity>? Timings { get; set; }
    }
}
