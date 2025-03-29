using System.ComponentModel.DataAnnotations;
using ChampionsLeagueMaster.Models;

namespace ChampionsLeagueMaster.ViewModels.Teams
{
    public class TeamViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa zespołu")]
        public string? Name { get; set; }

        [Display(Name = "Kraj")]
        public string? Country { get; set; }

        [Display(Name = "Rok założenia")]
        public int FoundedAt { get; set; }
    }
}