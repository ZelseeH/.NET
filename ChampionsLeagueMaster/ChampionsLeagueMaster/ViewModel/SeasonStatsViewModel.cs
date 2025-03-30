using System.ComponentModel.DataAnnotations;

namespace ChampionsLeagueMaster.ViewModels.SeasonStats
{
    public class SeasonStatsViewModel
    {
        [Display(Name = "Pozycja")]
        public int Position { get; set; }

        public int TeamId { get; set; }

        [Display(Name = "Drużyna")]
        public string? TeamName { get; set; }

        [Display(Name = "Rozegrane mecze")]
        public int? Played { get; set; }

        [Display(Name = "Wygrane")]
        public int? Wins { get; set; }

        [Display(Name = "Remisy")]
        public int? Draws { get; set; }

        [Display(Name = "Porażki")]
        public int? Losses { get; set; }

        [Display(Name = "Bramki zdobyte")]
        public int? GoalsScored { get; set; }

        [Display(Name = "Bramki stracone")]
        public int? GoalsConceded { get; set; }

        [Display(Name = "Różnica bramek")]
        public int? GoalDifference => GoalsScored - GoalsConceded;

        [Display(Name = "Punkty")]
        public int? Points { get; set; }
    }
}
