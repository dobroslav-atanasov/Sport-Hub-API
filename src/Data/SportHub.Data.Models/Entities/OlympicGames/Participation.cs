namespace SportHub.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Participations", Schema = "dbo")]
public class Participation : BaseDeletableEntity<Guid>, IEquatable<Participation>
{
    public int Code { get; set; }

    public Guid? AthleteId { get; set; }
    public virtual Athlete Athlete { get; set; }

    public Guid? EventId { get; set; }
    public virtual Event Event { get; set; }

    public int? NOCId { get; set; }
    public virtual NOC NOC { get; set; }

    public Guid? ResultId { get; set; }
    public virtual Result Result { get; set; }

    public int? AgeYears { get; set; }

    public int? AgeDays { get; set; }

    public int? MedalId { get; set; }
    public virtual Medal Medal { get; set; }

    public int? FinishTypeId { get; set; }
    public virtual FinishType FinishType { get; set; }

    public bool IsCoach { get; set; } = false;

    public virtual ICollection<Squad> Squads { get; set; } = new HashSet<Squad>();

    public bool Equals(Participation other)
    {
        var equals = true;

        if (this.AgeYears != other.AgeYears)
        {
            other.AgeYears = this.AgeYears;
            equals = false;
        }

        if (this.AgeDays != other.AgeDays)
        {
            other.AgeDays = this.AgeDays;
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

        if (this.IsCoach != other.IsCoach)
        {
            other.IsCoach = this.IsCoach;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Participation && this.Equals((Participation)obj);
    }
}