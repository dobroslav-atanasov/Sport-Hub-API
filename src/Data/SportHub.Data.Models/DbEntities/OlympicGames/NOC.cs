namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("NOCs", Schema = "dbo")]
public class NOC : BaseDeletableEntity<int>, IEquatable<NOC>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string OfficialName { get; set; }

    [Required]
    [StringLength(3)]
    public string Code { get; set; }

    public int? Created { get; set; }

    public int? Recognized { get; set; }

    public int? Disbanded { get; set; }

    [StringLength(10)]
    public string WorldCode { get; set; }

    [MaxLength(100)]
    public string Capital { get; set; }

    [MaxLength(50)]
    public string Continent { get; set; }

    [MaxLength(20)]
    public string Abbreviation { get; set; }

    [StringLength(3)]
    public string RelatedCode { get; set; }

    [MaxLength(10000)]
    public string Description { get; set; }

    public byte[] Flag { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();

    public virtual ICollection<President> Presidents { get; set; } = new HashSet<President>();

    public virtual ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

    public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();

    public bool Equals(NOC other)
    {
        var equals = true;
        if (this.OfficialName != other.OfficialName)
        {
            other.OfficialName = this.OfficialName;
            equals = false;
        }

        if (this.Created != other.Created)
        {
            other.Created = this.Created;
            equals = false;
        }

        if (this.Disbanded != other.Disbanded)
        {
            other.Disbanded = this.Disbanded;
            equals = false;
        }

        if (this.Recognized != other.Recognized)
        {
            other.Recognized = this.Recognized;
            equals = false;
        }

        if (this.Abbreviation != other.Abbreviation)
        {
            other.Abbreviation = this.Abbreviation;
            equals = false;
        }

        if (this.RelatedCode != other.RelatedCode)
        {
            other.RelatedCode = this.RelatedCode;
            equals = false;
        }

        if (this.Description != other.Description)
        {
            other.Description = this.Description;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return (obj is NOC) && this.Equals((NOC)obj);
    }
}