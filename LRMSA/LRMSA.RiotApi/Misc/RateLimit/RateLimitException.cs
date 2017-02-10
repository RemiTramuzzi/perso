using System;

namespace LRMSA.RiotApi.Misc.RateLimit
{
    public class RateLimitException : Exception
    {
        public int Time { get; set; }

        public RateLimitException(int time)
        {
            Time = time;
        }
    }
}