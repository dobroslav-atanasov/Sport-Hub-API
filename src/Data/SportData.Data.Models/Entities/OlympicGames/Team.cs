namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("Teams", Schema = "dbo")]
public class Team : BaseDeletableEntity<Guid>, IEquatable<Team>
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public Guid? EventId { get; set; }
    public virtual Event Event { get; set; }

    public int? NOCId { get; set; }
    public virtual NOC NOC { get; set; }

    public Guid? ResultId { get; set; }
    public virtual Result Result { get; set; }

    public Guid? CoachId { get; set; }
    public virtual Athlete Coach { get; set; }

    public int? MedalId { get; set; }
    public virtual Medal Medal { get; set; }

    public int? FinishTypeId { get; set; }
    public virtual FinishType FinishType { get; set; }

    public virtual ICollection<Squad> Squads { get; set; } = new HashSet<Squad>();

    public bool Equals(Team other)
    {
        var equals = true;

        if (this.CoachId != other.CoachId)
        {
            other.CoachId = this.CoachId;
            equals = false;
        }

        if (this.MedalId != other.MedalId)
        {
            other.MedalId = this.MedalId;
            equals = false;
        }

        if (this.FinishTypeId != other.FinishTypeId)
        {
            other.FinishTypeId = this.FinishTypeId;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return (obj is Team) && this.Equals((Team)obj);
    }
}