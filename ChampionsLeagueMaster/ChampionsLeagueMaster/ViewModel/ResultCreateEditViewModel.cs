using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChampionsLeagueMaster.ViewModels.Results
{
    public class ResultCreateEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Wybierz drużynę gospodarzy")]
        [Display(Name = "Gospodarz")]
        public int? HomeTeamId { get; set; }

        [Required(ErrorMessage = "Wybierz drużynę gości")]
        [Display(Name = "Gość")]
        public int? AwayTeamId { get; set; }

        [Required(ErrorMessage = "Podaj sezon")]
        [Display(Name = "Sezon")]
        public string? Season { get; set; }

        [Display(Name = "Bramki gospodarza")]
        [Range(0, 50, ErrorMessage = "Liczba bramek musi być między 0 a 50")]
        public int? HomeTeamGoals { get; set; } = 0;

        [Display(Name = "Bramki gościa")]
        [Range(0, 50, ErrorMessage = "Liczba bramek musi być między 0 a 50")]
        public int? AwayTeamGoals { get; set; } = 0;

        [Display(Name = "Data meczu")]
        [DataType(DataType.Date)]
        public DateOnly? MatchDay { get; set; }

        [Display(Name = "Godzina meczu")]
        [DataType(DataType.Time)]
        public TimeOnly? MatchTime { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Runda")]
        public string? Round { get; set; }

        
        public SelectList? Teams { get; set; }

       
        public List<string> AvailableStatuses { get; set; } = new List<string>
        {
            "Zaplanowany",
            "W trakcie",
            "Zakończony",
            "Odwołany"
        };

        public List<string> AvailableRounds { get; set; } = new List<string>
        {

            "1. Kolejka",
            "2. Kolejka",
            "3. Kolejka",
            "4. Kolejka",
            "5. Kolejka",
            "6. Kolejka",
            "7. Kolejka",
            "8. Kolejka",
            "1/16 finału",
            "1/8 finału",
            "Ćwierćfinał",
            "Półfinał",
            "Finał"
        };
    }
}
