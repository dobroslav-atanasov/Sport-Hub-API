namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Persons", Schema = "dbo")]
public class Person : BaseDeletableEntity<Guid>, IEquatable<Person>
{
    [Required]
    [MaxLength(20)]
    public string Code { get; set; }

    [Required]
    [MaxLength(200)]
    public string OriginalCode { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string TvName { get; set; }

    [MaxLength(200)]
    public string ShortName { get; set; }

    [Required]
    [MaxLength(10)]
    public string Gender { get; set; }

    [DataType("datetime2")]
    public DateTime? BirthDate { get; set; }

    public int Height { get; set; }

    public int Weight { get; set; }

    [MaxLength(50)]
    public string Category { get; set; }

    public bool Equals(Person other)
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

        if (this.BirthDate != other.BirthDate)
        {
            other.BirthDate = this.BirthDate;
            equals = false;
        }

        if (this.Height != other.Height)
        {
            other.Height = this.Height;
            equals = false;
        }

        if (this.Weight != other.Weight)
        {
            other.Weight = this.Weight;
            equals = false;
        }

        if (this.Category != other.Category)
        {
            other.Category = this.Category;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Person && this.Equals((Person)obj);
    }
}