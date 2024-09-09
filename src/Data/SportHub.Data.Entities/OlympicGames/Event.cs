namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Events", Schema = "dbo")]
public class Event : BaseDeletableEntity<Guid>, IEquatable<Event>
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [StringLength(1)]
    public string Gender { get; set; }

    [Required]
    [MaxLength(200)]
    public string LongName { get; set; }

    [Required]
    [MaxLength(200)]
    public string ShortName { get; set; }

    [MaxLength(50)]
    public string OriginalCode { get; set; }

    [Required]
    public int Order { get; set; }

    [Required]
    public bool IsTeam { get; set; } = false;

    public int DisciplineId { get; set; }
    public virtual Discipline Discipline { get; set; }

    public int GameId { get; set; }
    public virtual Game Game { get; set; }

    public virtual ICollection<Phase> Phases { get; set; } = new HashSet<Phase>();

    public virtual ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

    public virtual ICollection<Record> Records { get; set; } = new HashSet<Record>();

    public bool Equals(Event other)
    {
        var equal = true;

        if (this.Order != other.Order)
        {
            other.Order = this.Order;
            equal = false;
        }

        return equal;
    }

    public override bool Equals(object obj)
    {
        return obj is Event && this.Equals((Event)obj);
    }
}