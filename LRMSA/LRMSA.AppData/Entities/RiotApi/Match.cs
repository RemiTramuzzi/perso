using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LRMSA.AppData.Entities.RiotApi
{
    public class Match
    {
        public long Id { get; set; }

        [Index(IsUnique = true)]
        public long RiotId { get; set; }

        public DateTime PlayedOn { get; set; }

        public virtual ICollection<SummonerMatch> Summoners { get; set; }
    }
}