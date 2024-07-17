namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SportHub.Data.Common.Models;
using SportHub.Data.Models.Enumerations.OlympicGames;

[Table("Teams", Schema = "dbo")]
public class Team : BaseDeletableEntity<Guid>, IEquatable<Team>
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    public Guid EventId { get; set; }
    public virtual Event Event { get; set; }

    [Required]
    public int NationalOlympicCommitteeId { get; set; }
    public virtual NationalOlympicCommittee NationalOlympicCommittee { get; set; }

    public Guid? ResultId { get; set; }
    public virtual Result Result { get; set; }

    public Guid? CoachId { get; set; }
    public virtual Athlete Coach { get; set; }

    public MedalEnum Medal { get; set; }

    public FinishStatusEnum FinishStatus { get; set; }

    public virtual ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

    public bool Equals(Team other)
    {
        var equals = true;

        if (this.CoachId != other.CoachId)
        {
            other.CoachId = this.CoachId;
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

        return equals;
    }

    public override bool Equals(object obj)
    {
        return (obj is Team) && this.Equals((Team)obj);
    }
}