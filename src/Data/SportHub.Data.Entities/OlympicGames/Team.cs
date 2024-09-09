namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Teams", Schema = "dbo")]
public class Team : BaseDeletableEntity<Guid>, IEquatable<Team>
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string TvName { get; set; }

    [MaxLength(200)]
    public string ShortName { get; set; }

    [Required]
    [StringLength(1)]
    public string Gender { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }

    [MaxLength(10)]
    public string Organisation { get; set; }

    [MaxLength(50)]
    public string OriginalCode { get; set; }

    public int Medal { get; set; }

    /// <summary>
    /// Highlights
    /// </summary>
    [MaxLength(100)]
    public string HighlightsType { get; set; }

    [MaxLength(10000)]
    public string Highlights { get; set; }

    [MaxLength(10000)]
    public string AddInformation { get; set; }

    public virtual ICollection<Participation> Athletes { get; set; } = new HashSet<Participation>();

    public virtual ICollection<Coach> Coaches { get; set; } = new HashSet<Coach>();

    public bool Equals(Team other)
    {
        var equals = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.TvName != other.TvName)
        {
            other.TvName = this.TvName;
            equals = false;
        }

        if (this.ShortName != other.ShortName)
        {
            other.ShortName = this.ShortName;
            equals = false;
        }

        if (this.Gender != other.Gender)
        {
            other.Gender = this.Gender;
            equals = false;
        }

        if (this.Type != other.Type)
        {
            other.Type = this.Type;
            equals = false;
        }

        if (this.Organisation != other.Organisation)
        {
            other.Organisation = this.Organisation;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Team && this.Equals((Team)obj);
    }
}