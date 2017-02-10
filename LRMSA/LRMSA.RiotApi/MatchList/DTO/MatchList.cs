using System.Collections.Generic;

namespace LRMSA.RiotApi.MatchList.DTO
{
    public class MatchList
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int TotalGames { get; set; }
        public List<MatchReference> Matches { get; set; }
    }
}