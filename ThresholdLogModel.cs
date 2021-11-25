using System;

namespace EpThreshold
{
    public class ThresholdLogModel
    {
        public string Url { get; set; }
        public int StatusCode { get; set; }
        public string Method { get; set; }
        public long TimeMs { get; set; }
        public DateTimeOffset Started { get; set; }
    }
}
