using System.Timers;

namespace LRMSA.RiotApi.Misc.RateLimit
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class RateLimitTimer : Timer
    {
        private int Counter { get; set; }
        public readonly RateLimit RateLimit;

        public RateLimitTimer(RateLimit rateLimit)
            : base(rateLimit.Time * 1000)
        {
            AutoReset = false;
            RateLimit = rateLimit;
            Elapsed += (sender, args) =>
            {
                Counter = 0;
                Stop();
            };
        }

        public void Increment()
        {
            if (Counter == RateLimit.Limit)
                throw new RateLimitException(RateLimit.Time);

            if (!Enabled)
                Start();

            Counter++;
        }
    }
}