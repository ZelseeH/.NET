using System.ComponentModel.DataAnnotations;

namespace ChampionsLeagueMaster.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? TeamId { get; set; }
        public string? Position { get; set; }
        public int? JerseyNumber { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Country { get; set; }

        public Team? Team { get; set; }     }
}
