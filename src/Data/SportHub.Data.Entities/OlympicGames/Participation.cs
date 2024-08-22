namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Participations", Schema = "dbo")]
public class Participation : BaseDeletableEntity<Guid>, IEquatable<Participation>
{
    [Required]
    [MaxLength(20)]
    public string Code { get; set; }

    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; }

    public Guid EventId { get; set; }
    public virtual Event Event { get; set; }

    public int NOCId { get; set; }
    public virtual NOC NOC { get; set; }

    public bool Equals(Participation other)
    {
        var equals = true;

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Participation && this.Equals((Participation)obj);
    }
}