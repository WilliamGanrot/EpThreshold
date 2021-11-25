using EpThreshold;
using System;

namespace EpThreshold
{
    public class EpThresholdOptions
    {
        public int Threshold { get; set; } = 1500;

        public Action<ThresholdLogModel> ThresholdMetAction = null;
    }

}
