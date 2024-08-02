namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Events", Schema = "dbo")]
public class Event : BaseDeletableEntity<Guid>, IEquatable<Event>
{
    /// <summary>
    /// description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    /// <summary>
    /// long description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string LongName { get; set; }

    /// <summary>
    /// seo description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string SEOName { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    /// <summary>
    /// code
    /// </summary>
    [MaxLength(50)]
    public string OriginalCode { get; set; }

    /// <summary>
    /// order
    /// </summary>
    [Required]
    public int Order { get; set; }

    /// <summary>
    /// is team
    /// </summary>
    [Required]
    public bool IsTeam { get; set; } = false;

    public int DisciplineId { get; set; }
    public virtual Discipline Discipline { get; set; }

    // phases
    public virtual ICollection<Phase> Phases { get; set; } = new HashSet<Phase>();

    public bool Equals(Event other)
    {
        var equal = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equal = false;
        }

        if (this.LongName != other.LongName)
        {
            other.LongName = this.LongName;
            equal = false;
        }

        if (this.SEOName != other.SEOName)
        {
            other.SEOName = this.SEOName;
            equal = false;
        }

        if (this.Order != other.Order)
        {
            other.Order = this.Order;
            equal = false;
        }

        if (this.IsTeam != other.IsTeam)
        {
            other.IsTeam = this.IsTeam;
            equal = false;
        }

        return equal;
    }

    public override bool Equals(object obj)
    {
        return obj is Event && this.Equals((Event)obj);
    }
}