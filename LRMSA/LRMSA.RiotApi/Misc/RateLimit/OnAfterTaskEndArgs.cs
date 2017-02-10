using System;

namespace LRMSA.RiotApi.Misc.RateLimit
{
    public class OnAfterTaskEndArgs : EventArgs
    {
        public Uri Uri { get; set; }
        public RateLimitList RateLimitList { get; set; }
    }
}