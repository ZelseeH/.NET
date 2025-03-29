using System.Collections.Generic;
using ChampionsLeagueMaster.Models;

namespace ChampionsLeagueMaster.ViewModels.Players
{
    public class PlayerListViewModel
    {
        public PaginatedList<PlayerViewModel>? Players { get; set; }
        public string? CurrentTeamFilter { get; set; }
        public string? CurrentPositionFilter { get; set; }
        public string? CurrentSortOrder { get; set; }
        public List<string>? AvailablePositions { get; set; }
    }
}
