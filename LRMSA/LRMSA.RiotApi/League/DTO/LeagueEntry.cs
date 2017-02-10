namespace LRMSA.RiotApi.League.DTO
{
    public class LeagueEntry
    {
        public string Division { get; set; }
        public bool IsFreshBlood { get; set; }
        public bool IsHotStreak { get; set; }
        public bool IsInactive { get; set; }
        public bool IsVeteran { get; set; }
        public int LeaguePoints { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public MiniSeries MiniSeries { get; set; }

        public string PlayerOrTeamId { get; set; }
        public string PlayerOrTeamName { get; set; }

        public string PlayStyle { get; set; } // NONE, SOLO, SQUAD, TEAM
    }
}