using System;
using LRMSA.RiotApi.MatchList.DTO;

namespace LRMSA.RiotApi.MatchList.Parameters
{
    public class BySummonerParameters
    {
        public long[] ChampionIds { get; set; }
        public RankedQueues[] RankedQueues { get; set; }
        public string[] Seasons { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? BeginIndex { get; set; }
        public int? EndIndex { get; set; }
    }
}