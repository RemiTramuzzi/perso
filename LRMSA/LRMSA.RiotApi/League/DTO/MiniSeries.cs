namespace LRMSA.RiotApi.League.DTO
{
    public class MiniSeries
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Progress { get; set; } // W = win, L = Loss, N = Not played yet
        public int Target { get; set; } // Number of wins required for promotion
    }
}