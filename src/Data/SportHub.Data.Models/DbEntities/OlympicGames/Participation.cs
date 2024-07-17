namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SportHub.Data.Common.Models;
using SportHub.Data.Models.Enumerations.OlympicGames;

[Table("Participations", Schema = "dbo")]
public class Participation : BaseDeletableEntity<Guid>, IEquatable<Participation>
{
    [Required]
    public int Code { get; set; }

    [Required]
    public Guid AthleteId { get; set; }
    public virtual Athlete Athlete { get; set; }

    [Required]
    public Guid EventId { get; set; }
    public virtual Event Event { get; set; }

    [Required]
    public int NationalOlympicCommitteeId { get; set; }
    public virtual NationalOlympicCommittee NationalOlympicCommittee { get; set; }

    public Guid? ResultId { get; set; }
    public virtual Result Result { get; set; }

    public int? AgeYears { get; set; }

    public int? AgeDays { get; set; }

    [Required]
    public MedalEnum Medal { get; set; }

    [Required]
    public FinishStatusEnum FinishStatus { get; set; }

    public bool IsCoach { get; set; } = false;

    public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();

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

        if (this.Medal != other.Medal)
        {
            other.Medal = this.Medal;
            equals = false;
        }

        if (this.FinishStatus != other.FinishStatus)
        {
            other.FinishStatus = this.FinishStatus;
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