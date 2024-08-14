namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Venues", Schema = "dbo")]
public class Venue : BaseDeletableEntity<Guid>
{
    [MaxLength(500)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Code { get; set; }

    public virtual ICollection<Unit> Units { get; set; } = new HashSet<Unit>();
}