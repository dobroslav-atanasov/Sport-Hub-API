namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("Clubs", Schema = "dbo")]
public class Club : BaseDeletableEntity<int>, IEquatable<Club>
{
    [Required]
    public int Code { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string City { get; set; }

    [MaxLength(10)]
    public string Country { get; set; }

    public virtual ICollection<AthleteClub> AthletesClubs { get; set; } = new HashSet<AthleteClub>();

    public bool Equals(Club other)
    {
        var equals = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            return false;
        }

        if (this.City != other.City)
        {
            other.City = this.City;
            return false;
        }

        if (this.Country != other.Country)
        {
            other.Country = this.Country;
            return false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return (obj is Club) && this.Equals((Club)obj);
    }
}