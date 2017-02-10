using System;
using System.Linq;
using System.Net.Http;

namespace LRMSA.RiotApi.Misc.RateLimit
{
    public class HttpRateLimitedResponseMessage
    {
        public readonly HttpResponseMessage HttpResponseMessage;
        public readonly RateLimitList HeaderRateLimits;

        private const string RateLimitHttpHeader = "X-Rate-Limit-Count";

        public HttpRateLimitedResponseMessage(HttpResponseMessage response)
        {
            HttpResponseMessage = response;
            HeaderRateLimits = HttpResponseMessage.Headers.GetValue(RateLimitHttpHeader, RateLimitList.FromRiotApiHeader);
            if (HeaderRateLimits == null)
                throw new Exception("Pas de header " + RateLimitHttpHeader + " pour ce HttpResponseMessage");
        }

        public int? TimeToWaitForNext(RateLimitList rateLimits)
        {
            foreach (var headerLimit in HeaderRateLimits.OrderByDescending(hrl => hrl.Time))
            {
                var rateLimit = rateLimits[headerLimit.Time];
                if (rateLimit == null)
                    continue;

                if (headerLimit.Limit < rateLimit.Limit)
                    continue;
                
                return headerLimit.Time;
            }

            return null;
        }
    }
}