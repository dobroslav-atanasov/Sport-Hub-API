namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Units", Schema = "dbo")]
public class Unit : BaseDeletableEntity<Guid>, IEquatable<Unit>
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string LongName { get; set; }

    [MaxLength(200)]
    public string ShortName { get; set; }

    [MaxLength(50)]
    public string OriginalCode { get; set; }

    [MaxLength(10)]
    public string Type { get; set; }

    [MaxLength(10)]
    public string Scheduled { get; set; }

    [MaxLength(20)]
    public string Number { get; set; }

    [MaxLength(10)]
    public string Order { get; set; }

    [DataType("DATETIME2")]
    public DateTime? OlympicDay { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool HasMedal { get; set; } = false;

    public string Status { get; set; }

    public Guid PhaseId { get; set; }
    public virtual Phase Phase { get; set; }

    [MaxLength(20)]
    public string VenueCode { get; set; }

    [MaxLength(200)]
    public string VenueName { get; set; }

    [MaxLength(20)]
    public string LocationCode { get; set; }

    [MaxLength(200)]
    public string LocationName { get; set; }

    [MaxLength(100)]
    public string Session { get; set; }

    [MaxLength(100)]
    public string Group { get; set; }

    public bool Equals(Unit other)
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

        if (this.Type != other.Type)
        {
            other.Type = this.Type;
            equal = false;
        }

        return equal;
    }

    public override bool Equals(object obj)
    {
        return obj is Unit && this.Equals((Unit)obj);
    }
}