using System;
using System.Collections.Generic;
using System.Linq;

namespace LRMSA.RiotApi.Misc.RateLimit
{
    public class RateLimitList : List<RateLimit>
    {
        public new RateLimit this[int index]
        {
            get { return this.FirstOrDefault(rl => rl.Time == index); }
            set
            {
                if (this[index] != null)
                    throw new Exception("There is already a RateLimit with Time = " + index);

                Add(value);
            }
        }

        public RateLimitList()
        {
        }

        public RateLimitList(IEnumerable<RateLimit> rateLimits)
        {
            var rateLimitsArray = rateLimits.ToArray();

            if (rateLimitsArray.GroupBy(rl => rl.Time).Any(g => g.Count() > 1))
                throw new Exception("Plusieurs RateLimit avec la même clé Time");

            foreach (var rl in rateLimitsArray)
                Add(rl);
        }

        public static RateLimitList FromRiotApiHeader(string header)
        {
            return new RateLimitList(header.Split(',')
                .Select(p => new RateLimit
                {
                    Time = int.Parse(p.Substring(p.IndexOf(':') + 1)),
                    Limit = int.Parse(p.Substring(0, p.IndexOf(':')))
                }));
        }
    }
}