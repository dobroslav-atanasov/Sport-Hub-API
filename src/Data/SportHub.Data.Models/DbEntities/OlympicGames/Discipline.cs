namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Disciplines", Schema = "dbo")]
public class Discipline : BaseDeletableEntity<int>, IEquatable<Discipline>
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(5)]
    public string Code { get; set; }

    [Required]
    [MaxLength(50)]
    public string Sport { get; set; }

    [Required]
    [MaxLength(5)]
    public string SportCode { get; set; }

    [Required]
    [MaxLength(50)]
    public string SEOName { get; set; }

    [Required]
    public bool IsHistoric { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();

    public bool Equals(Discipline other)
    {
        var equals = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.Sport != other.Sport)
        {
            other.Sport = this.Sport;
            equals = false;
        }

        if (this.SportCode != other.SportCode)
        {
            other.SportCode = this.SportCode;
            equals = false;
        }

        if (this.SEOName != other.SEOName)
        {
            other.SEOName = this.SEOName;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Discipline && this.Equals((Discipline)obj);
    }
}