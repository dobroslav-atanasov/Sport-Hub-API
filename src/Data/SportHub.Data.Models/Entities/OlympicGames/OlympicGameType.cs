namespace SportHub.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("OlympicGameTypes", Schema = "dbo")]
public class OlympicGameType : BaseDeletableEntity<int>
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();

    public virtual ICollection<Sport> Sports { get; set; } = new HashSet<Sport>();
}