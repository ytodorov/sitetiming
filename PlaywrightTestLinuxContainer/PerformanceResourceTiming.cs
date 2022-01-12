namespace PlaywrightTestLinuxContainer
{
    public record PerformanceResourceTiming
    {
        /// <summary>
        /// Gets or sets Duration in milliseconds.
        /// </summary>
        public double Duration { get; set; }

        public string Name { get; set; }

        public double ResponseStart { get; set; }

        public double ResponseEnd { get; set; }

        /// <summary>
        /// Gets ir sets ServerTime_Ttfb: Waiting (TTFB). The browser is waiting for the first byte of a response.
        /// TTFB stands for Time To First Byte.
        /// This timing includes 1 round trip of latency and the time the server took to prepare the response.
        /// Measured in milliseconds.
        /// </summary>
        public double ServerTime_Ttfb
        {
            get
            {
                double result = Math.Round(ResponseStart, 2);
                return result;
            }
        }

        /// <summary>
        /// Gets ContentDownload. The browser is receiving the response. Measured in milliseconds.
        /// </summary>
        public double ContentDownload
        {
            get
            {
                double result = Math.Round(ResponseEnd - ResponseStart, 2);

                if (ResponseStart == 0)
                {
                    result = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets EncodedBodySize in bytes.
        /// </summary>
        public int EncodedBodySize { get; set; }

        public int EncodedBodySizeInKiloBytes { get; set; }

        /// <summary>
        /// Gets or sets TransferSize in bytes.
        /// </summary>
        public int TransferSize { get; set; }

        public int TransferSizeInKylobytes { get; set; }

        public override string ToString()
        {
            return $"{Name} {Duration}";
        }
    }
}
