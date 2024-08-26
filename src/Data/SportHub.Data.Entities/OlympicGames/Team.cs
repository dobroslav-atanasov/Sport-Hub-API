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

    public virtual ICollection<Participation> Athletes { get; set; } = new HashSet<Participation>();

    public bool Equals(Team other)
    {
        var equal = true;

        //if (this.Order != other.Order)
        //{
        //    other.Order = this.Order;
        //    equal = false;
        //}

        return equal;
    }

    public override bool Equals(object obj)
    {
        return obj is Team && this.Equals((Team)obj);
    }
}