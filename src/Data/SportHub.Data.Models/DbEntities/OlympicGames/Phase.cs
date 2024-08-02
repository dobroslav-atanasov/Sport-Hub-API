namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Phases", Schema = "dbo")]
public class Phase : BaseDeletableEntity<Guid>, IEquatable<Phase>
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
    /// short description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string ShortName { get; set; }

    /// <summary>
    /// code
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    /// <summary>
    /// order
    /// </summary>
    [Required]
    public int Order { get; set; }

    [Required]
    public Guid EventId { get; set; }
    public virtual Event Event { get; set; }

    public bool Equals(Phase other)
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

        if (this.ShortName != other.ShortName)
        {
            other.ShortName = this.ShortName;
            equal = false;
        }

        if (this.Order != other.Order)
        {
            other.Order = this.Order;
            equal = false;
        }

        return equal;
    }

    public override bool Equals(object obj)
    {
        return obj is Phase && this.Equals((Phase)obj);
    }
}