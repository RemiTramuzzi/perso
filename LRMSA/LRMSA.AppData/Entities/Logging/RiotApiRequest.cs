using System;
using System.Collections.Generic;

namespace LRMSA.AppData.Entities.Logging
{
    public class RiotApiRequest
    {
        public long Id { get; set; }

        public string AbsolutePath { get; set; }
        public DateTime RespondedOn { get; set; }

        public virtual ICollection<RateLimitState> RateLimitStates { get; set; }
    }
}
