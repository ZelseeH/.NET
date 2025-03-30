using System.Collections.Generic;

namespace ChampionsLeagueMaster.ViewModels.Results
{
    public class ResultListViewModel
    {
        public List<ResultViewModel> Results { get; set; } = new List<ResultViewModel>();
        public List<string> Seasons { get; set; } = new List<string>();
        public List<string> Rounds { get; set; } = new List<string>();
        public string? SelectedSeason { get; set; }
        public string? SelectedRound { get; set; }
    }
}
