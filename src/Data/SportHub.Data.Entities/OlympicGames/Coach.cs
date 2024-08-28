namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Coaches", Schema = "dbo")]
public class Coach : BaseDeletableEntity<Guid>
{
    public Guid TeamId { get; set; }
    public virtual Team Team { get; set; }

    public Guid CoachId { get; set; }

    public int Order { get; set; }
}