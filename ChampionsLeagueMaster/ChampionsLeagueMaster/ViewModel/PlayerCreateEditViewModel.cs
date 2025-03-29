using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChampionsLeagueMaster.ViewModels.Players
{
    public class PlayerCreateEditViewModel
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Imię jest wymagane")]
        [Display(Name = "Imię")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [Display(Name = "Nazwisko")]
        public string? LastName { get; set; }

        [Display(Name = "Zespół")]
        public int? TeamId { get; set; }
        [Required(ErrorMessage = "Pozycja jest wymagana")]
        [Display(Name = "Pozycja")]
        public string? Position { get; set; }
        [Required(ErrorMessage = "Numer koszulki jest wymagany")]
                [Display(Name = "Numer na koszulce")]
        [Range(1, 99, ErrorMessage = "Numer musi być między 1 a 99")]
        public int? JerseyNumber { get; set; }

        [Required(ErrorMessage = "Data Urodzenia jest wymagana")]
        [Display(Name = "Data urodzenia")]
        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; }

        [Required(ErrorMessage = "Kraj pochodzenia jest wymagany")]
        [Display(Name = "Kraj")]
        public string? Country { get; set; }

        public SelectList? Teams { get; set; }
    }
}
