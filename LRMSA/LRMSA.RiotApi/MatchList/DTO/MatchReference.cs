namespace LRMSA.RiotApi.MatchList.DTO
{
    public class MatchReference
    {
        public long Champion { get; set; }
        public long MatchId { get; set; }
        public long Timestamp { get; set; }
        public string Lane { get; set; } // MID, MIDDLE, TOP, JUNGLE, BOT, BOTTOm
        public string PlatformId { get; set; }
        public string Queue { get; set; } // RANKED_FLEX_SR, RANKED_SOLO_5x5, RANKED_TEAM_3x3, RANKED_TEAM_5x5, TEAM_BUILDER_DRAFT_RANKED_5x5, TEAM_BUILDER_RANKED_SOLO
        public string Region { get; set; } // br, eune, euw, jp, kr, lan, las, na, oce, ru, tr
        public string Role { get; set; } // DUO, NONE, SOLO, DUO_CARRY, DUO_SUPPORT
        public string Season { get; set; } // PRESEASON3, SEASON3, PRESEASON2014, SEASON2014, PRESEASON2015, SEASON2015, PRESEASON2016, SEASON2016, PRESEASON2017, SEASON2017
    }
}