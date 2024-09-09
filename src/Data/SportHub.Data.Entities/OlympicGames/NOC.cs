namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("NOCs", Schema = "dbo")]
public class NOC : BaseDeletableEntity<int>, IEquatable<NOC>
{
    [Required]
    [StringLength(3)]
    public string Code { get; set; }

    /// <summary>
    /// description
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// longDescription, officialNocName(membership)
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string OfficialName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string OfficialCommitteeName { get; set; }

    [MaxLength(50)]
    public string CommitteeAbbreviation { get; set; }

    /// <summary>
    /// seodescription
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string SEOName { get; set; }

    /// <summary>
    /// descriptionOrder
    /// </summary>
    public int NameOrder { get; set; }

    /// <summary>
    /// longDescriptionOrder
    /// </summary>
    public int FullNameOrder { get; set; }

    /// <summary>
    /// medal Y - yes, N - no ??????
    /// </summary>
    [Required]
    public bool IsMedal { get; set; }

    /// <summary>
    /// note "H" - history, "P" - participate
    /// </summary>
    [Required]
    public bool IsHistoric { get; set; }

    /// <summary>
    /// foundingDate (membership)
    /// </summary>
    public int? FoundingDate { get; set; }

    /// <summary>
    /// dateIOCRecognition (membership)
    /// </summary>
    public int? IOCRecognitionDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? DisbandedDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [StringLength(3)]
    public string RelatedNOC { get; set; }

    /// <summary>
    /// highlights (interest)
    /// </summary>
    [MaxLength(10000)]
    public string Highlights { get; set; }

    /// <summary>
    /// addInformation (interest)
    /// </summary>
    [MaxLength(5000)]
    public string Information { get; set; }

    /// <summary>
    /// firstOGAppearance (participation)
    /// </summary>
    public int? FirstAppearence { get; set; }

    /// <summary>
    /// numOGAppearance (participation)
    /// </summary>
    public int? Appearance { get; set; }

    /// <summary>
    /// summary (participation)
    /// </summary>
    [MaxLength(10000)]
    public string Summary { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [MaxLength(10000)]
    public string Description { get; set; }

    public virtual ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

    //public virtual ICollection<NOCAdministration> Administrations { get; set; } = new HashSet<NOCAdministration>();

    //public virtual ICollection<FlagBearer> FlagBearers { get; set; } = new HashSet<FlagBearer>();

    public bool Equals(NOC other)
    {
        var equals = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.OfficialName != other.OfficialName)
        {
            other.OfficialName = this.OfficialName;
            equals = false;
        }

        if (this.SEOName != other.SEOName)
        {
            other.SEOName = this.SEOName;
            equals = false;
        }

        if (this.IsMedal != other.IsMedal)
        {
            other.IsMedal = this.IsMedal;
            equals = false;
        }

        if (this.IsHistoric != other.IsHistoric)
        {
            other.IsHistoric = this.IsHistoric;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is NOC && this.Equals((NOC)obj);
    }
}