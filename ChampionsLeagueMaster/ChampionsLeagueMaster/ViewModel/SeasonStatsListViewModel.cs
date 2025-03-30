using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChampionsLeagueMaster.ViewModels.SeasonStats
{
    public class SeasonStatsListViewModel
    {
        public List<SeasonStatsViewModel>? Stats { get; set; } = new List<SeasonStatsViewModel>();

        [Display(Name = "Dostępne sezony")]
        public List<string>? Seasons { get; set; } = new List<string>();

        [Display(Name = "Wybrany sezon")]
        public string? SelectedSeason { get; set; }
    }
}
