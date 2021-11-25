using Microsoft.AspNetCore.Mvc.Filters;

namespace EpThreshold
{
    public class ThresholdAttribute : ActionFilterAttribute
    {
        public int Threshold { get; set; }
        public ThresholdAttribute(int threshold)
        {
            Threshold = threshold;
        }
    }
}
