using System;
using System.ComponentModel.DataAnnotations;

namespace ChampionsLeagueMaster.ViewModels.Results
{
    public class ResultViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Gospodarz")]
        public string? HomeTeamName { get; set; }

        [Display(Name = "Gość")]
        public string? AwayTeamName { get; set; }

        [Display(Name = "Sezon")]
        public string? Season { get; set; }

        [Display(Name = "Bramki gospodarza")]
        public int? HomeTeamGoals { get; set; }

        [Display(Name = "Bramki gościa")]
        public int? AwayTeamGoals { get; set; }

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

        // Pomocnicze właściwości
        public string Score => $"{HomeTeamGoals ?? 0} - {AwayTeamGoals ?? 0}";
        public string MatchDateTime => MatchDay.HasValue ?
            $"{MatchDay.Value.ToString("dd.MM.yyyy")} {(MatchTime.HasValue ? MatchTime.Value.ToString("HH:mm") : "")}" :
            "TBD";
    }
}
