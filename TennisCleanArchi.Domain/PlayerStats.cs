namespace TennisCleanArchi.Domain
{
    public class PlayerStats
    {
        public int Rank { get; set; }
        public int Points { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int Age { get; set; }
        public int[] Last { get; set; } = []; // Array of last match results (1 for win, 0 for loss)
    }
}
