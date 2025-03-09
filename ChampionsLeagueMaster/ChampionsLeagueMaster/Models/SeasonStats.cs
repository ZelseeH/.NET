namespace ChampionsLeagueMaster.Models
{
    public class SeasonStats
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public string? Season { get; set; }
        public int? Wins { get; set; }
        public int? Draws { get; set; }
        public int? Losses { get; set; }
        public int? Points { get; set; }
        public int? GoalsScored { get; set; }
        public int? GoalsConceded { get; set; }
        public int GoalDifference { get; private set; }

        public Team? Team { get; set; } // Changed to singular Team
    }
}
