namespace SportHub.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Medals", Schema = "dbo")]
public class Medal : BaseDeletableEntity<int>
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    public virtual ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

    public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();
}