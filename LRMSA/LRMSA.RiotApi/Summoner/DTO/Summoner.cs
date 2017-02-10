using System;

namespace LRMSA.RiotApi.Summoner.DTO
{
    public class Summoner
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int ProfileIconId { get; set; }
        public long RevisionDate { get; set; }

        public DateTime RevisionOn
        {
            get { return DateTimeOffset.FromUnixTimeMilliseconds(RevisionDate).DateTime; }
        }
        public long SummonerLevel { get; set; }
    }
}