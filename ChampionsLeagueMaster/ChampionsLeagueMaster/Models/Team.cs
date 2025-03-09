using System.ComponentModel.DataAnnotations;

namespace ChampionsLeagueMaster.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Country { get; set; }
        public int FoundedAt { get; set; }


        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<SeasonStats> SeasonStats { get; set; } = new List<SeasonStats>();
        public ICollection<Result> HomeMatches { get; set; } = new List<Result>();
        public ICollection<Result> AwayMatches { get; set; } = new List<Result>();
    }   
}
