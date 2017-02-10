using System;
using System.Collections.Generic;
using System.Linq;

namespace LRMSA.RiotApi.Misc.RateLimit
{
    public class RateLimitTimerList : List<RateLimitTimer>
    {
        public new RateLimitTimer this[int index]
        {
            get { return this.FirstOrDefault(t => t.RateLimit.Time == index); }
            set
            {
                if (this[index] != null)
                    throw new Exception("There is already a RateLimitTimer with RateLimit.Time = " + index);

                Add(value);
            }
        }
    }
}