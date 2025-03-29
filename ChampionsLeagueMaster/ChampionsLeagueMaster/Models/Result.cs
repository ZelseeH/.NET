using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChampionsLeagueMaster.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int? HomeTeamId { get; set; }
        public int? AwayTeamId { get; set; }
        public string? Season { get; set; }

        public int? HomeTeamGoals { get; set; } = 0;
        public int? AwayTeamGoals { get; set; } = 0;

        public DateOnly? MatchDay { get; set; } 
        public TimeOnly? MatchTime { get; set; } 

        public string? Status { get; set; }
       
        public string? Round { get; set; } 

        public Team? HomeTeam { get; set; } 
        public Team? AwayTeam { get; set; } 
    }
}
