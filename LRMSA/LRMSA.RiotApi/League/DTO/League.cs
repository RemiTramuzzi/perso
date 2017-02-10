using System.Collections.Generic;

namespace LRMSA.RiotApi.League.DTO
{
    public class League
    {
        public List<LeagueEntry> Entries { get; set; }
        public string Name { get; set; }
        public string Queue { get; set; } // RANKED_FLEX_SR, RANKED_FLEX_TT, RANKED_SOLO_5x5, RANKED_TEAM_3x3, RANKED_TEAM_5x5
        public string Tier { get; set; }
        public string ParticipantId { get; set; }
    }
}