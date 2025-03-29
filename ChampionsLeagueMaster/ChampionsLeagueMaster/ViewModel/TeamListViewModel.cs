using ChampionsLeagueMaster.Models;

namespace ChampionsLeagueMaster.ViewModels.Teams
{
    public class TeamListViewModel
    {
        public PaginatedList<TeamViewModel>? Teams { get; set; }
        public string? CurrentCountryFilter { get; set; }
        public string? CurrentSortOrder { get; set; }
        public List<string>? AvailableCountries { get; set; }
    }
}