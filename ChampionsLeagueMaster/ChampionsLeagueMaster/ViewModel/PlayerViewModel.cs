using System;
using System.ComponentModel.DataAnnotations;

namespace ChampionsLeagueMaster.ViewModels.Players
{
    public class PlayerViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Imię")]
        public string? FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string? LastName { get; set; }
        public int? TeamId { get; set; }


        [Display(Name = "Zespół")]
        public string? TeamName { get; set; }

        [Display(Name = "Pozycja")]
        public string? Position { get; set; }

        [Display(Name = "Numer na koszulce")]
        public int? JerseyNumber { get; set; }

        [Display(Name = "Data urodzenia")]
        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; }

        [Display(Name = "Kraj")]
        public string? Country { get; set; }
    }
}
