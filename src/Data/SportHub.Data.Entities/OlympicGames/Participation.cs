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

    [Required]
    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; }

    [Required]
    public Guid EventId { get; set; }
    public virtual Event Event { get; set; }

    [Required]
    public int NOCId { get; set; }
    public virtual NOC NOC { get; set; }

    public Guid? TeamId { get; set; }
    public virtual Team Team { get; set; }

    public int AgeYears { get; set; }

    public int AgeDays { get; set; }

    public int Medal { get; set; }

    public int Order { get; set; }

    public string Rank { get; set; }

    public string RankResultType { get; set; }

    public string RankType { get; set; }

    public bool RankEqual { get; set; } = false;

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