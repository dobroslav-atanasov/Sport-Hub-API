namespace SportHub.Data.Models.Entities.OlympicGames;

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

    [MinLength(10)]
    public string FlagCode { get; set; }

    public int? Created { get; set; }

    public int? Recognized { get; set; }

    public int? Disbanded { get; set; }

    [MaxLength(20)]
    public string Abbreviation { get; set; }

    [MaxLength(100)]
    public string ContinentalAssociation { get; set; }

    [StringLength(3)]
    public string RelatedNOCCode { get; set; }

    [MaxLength(10000)]
    public string Description { get; set; }

    public virtual ICollection<NOCPresident> NOCPresidents { get; set; } = new HashSet<NOCPresident>();

    public virtual ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

    public bool Equals(NOC other)
    {
        var equals = true;
        if (this.OfficialName != other.OfficialName)
        {
            other.OfficialName = this.OfficialName;
            equals = false;
        }

        if (this.FlagCode != other.FlagCode)
        {
            other.FlagCode = this.FlagCode;
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

        if (this.ContinentalAssociation != other.ContinentalAssociation)
        {
            other.ContinentalAssociation = this.ContinentalAssociation;
            equals = false;
        }

        if (this.RelatedNOCCode != other.RelatedNOCCode)
        {
            other.RelatedNOCCode = this.RelatedNOCCode;
            equals = false;
        }

        if (this.Description != other.Description)
        {
            other.Description = this.Description;
            equals = false;
        }

        if (this.NOCPresidents.Count != other.NOCPresidents.Count)
        {
            other.NOCPresidents = this.NOCPresidents;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return (obj is NOC) && this.Equals((NOC)obj);
    }
}