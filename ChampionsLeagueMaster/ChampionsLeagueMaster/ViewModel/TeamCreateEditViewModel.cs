using System.ComponentModel.DataAnnotations;

namespace ChampionsLeagueMaster.ViewModels.Teams
{
    public class TeamCreateEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa zespołu jest wymagana")]
        [Display(Name = "Nazwa zespołu")]
        [StringLength(100, ErrorMessage = "Nazwa zespołu nie może być dłuższa niż 100 znaków")]
        public string? Name { get; set; } 

        [Required(ErrorMessage = "Kraj jest wymagany")]
        [Display(Name = "Kraj")]
        [StringLength(60, ErrorMessage = "Nazwa kraju nie może być dłuższa niż 50 znaków")]
        public string? Country { get; set; } 

        [Required(ErrorMessage = "Rok założenia jest wymagany")]
        [Display(Name = "Rok założenia")]
        [Range(1800, 2025, ErrorMessage = "Rok założenia musi być między 1800 a 2025")]
        public int FoundedAt { get; set; }
    }

}
