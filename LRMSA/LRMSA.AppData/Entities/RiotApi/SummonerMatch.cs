using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LRMSA.AppData.Entities.RiotApi
{
    public class SummonerMatch
    {
        [Key, Column(Order = 0)]
        public long MatchId { get; set; }
        public virtual Match Match { get; set; }

        [Key, Column(Order = 1)]
        public long SummonerId { get; set; }
        public virtual Summoner Summoner { get; set; }

        public int ChampionId { get; set; }
        public virtual Champion Champion { get; set; }
    }
}