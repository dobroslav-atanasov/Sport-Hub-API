namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;

using global::SportHub.Data.Common.Models;

public class Unit : BaseDeletableEntity<Guid>, IEquatable<Unit>
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

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
    /// seo description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string SEOName { get; set; }

    /// <summary>
    /// code
    /// </summary>
    [MaxLength(50)]
    public string OriginalCode { get; set; }

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

        if (this.SEOName != other.SEOName)
        {
            other.SEOName = this.SEOName;
            equal = false;
        }

        return equal;
    }

    public override bool Equals(object obj)
    {
        return obj is Unit && this.Equals((Unit)obj);
    }
}