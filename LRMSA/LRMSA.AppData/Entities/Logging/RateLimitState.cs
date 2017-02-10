using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LRMSA.AppData.Entities.RiotApi;

namespace LRMSA.AppData.Entities.Logging
{
    public class RateLimitState
    {
        [Key, Column(Order = 0)]
        public long RiotApiRequestId { get; set; }
        public virtual RiotApiRequest RiotApiRequest { get; set; }

        [Key, Column(Order = 1)]
        public int RateLimitId { get; set; }
        public virtual RateLimit RateLimit { get; set; }

        public int CurrentState { get; set; }
    }
}